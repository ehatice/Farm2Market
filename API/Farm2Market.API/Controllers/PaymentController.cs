using Farm2Marrket.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe.Climate;
using Stripe;
using Farm2Marrket.Application.Sevices;
using Farm2Market.Domain.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly StripeSettings _stripeSettings;
	private readonly IProductService _productService;
	private readonly ICartService _cartService;

	public PaymentController(IOptions<StripeSettings> stripeSettings, IProductService orderService, ICartService cartService)
	{
		_stripeSettings = stripeSettings.Value;
		_productService = orderService;
		_cartService = cartService;
	}
	[Authorize(AuthenticationSchemes = "Bearer")]
	[HttpPost("CreateCheckoutSession")]
	public async Task<IActionResult> CreateCheckoutSession()
	{
		// Oturumdaki kullanıcı ID'sini alın (örneğin, Identity veya JWT üzerinden)
		var marketReceiverId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Kullanıcı kimliği doğrudan MarketReceiverId olarak kullanılıyor
		if (string.IsNullOrEmpty(marketReceiverId))
			return Unauthorized("Kullanıcı oturum açmamış.");

		// Kullanıcıya ait bekleyen siparişi alın
		var order = await _cartService.GetPendingOrderForUserAsync(marketReceiverId);
		if (order == null)
			return NotFound("Kullanıcı için bekleyen bir sipariş bulunamadı.");

		StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

		var lineItems = order.OrderItems.Select(item => new SessionLineItemOptions
		{
			PriceData = new SessionLineItemPriceDataOptions
			{
				Currency = "try",
				UnitAmount = Convert.ToInt32(item.Price * 100),
				ProductData = new SessionLineItemPriceDataProductDataOptions
				{
					Name = item.ProductName,
				}
			},
			Quantity = item.Quantity
		}).ToList();

		// Success ve Cancel URL parametrelerini ekleyin
		var successUrl = "https://yourdomain.com/payment-success?session_id={CHECKOUT_SESSION_ID}";
		var cancelUrl = "https://yourdomain.com/payment-cancelled";

		var options = new SessionCreateOptions
		{
			PaymentMethodTypes = new List<string> { "card" },
			LineItems = lineItems,
			Mode = "payment",
			Metadata = new Dictionary<string, string>
		{
			{ "OrderId", order.Id.ToString() },
			{ "MarketReceiverId", marketReceiverId }
		},
			SuccessUrl = successUrl,
			CancelUrl = cancelUrl
		};

		var service = new SessionService();
		var session = service.Create(options);

		return Ok(new { sessionId = session.Id, url = session.Url });
	}
	[Authorize(AuthenticationSchemes = "Bearer")]
	[HttpPost("StripeWebhook")]
	public async Task<IActionResult> StripeWebhook()
	{
		var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

		try
		{
			// Stripe'dan gelen event'i doğrula
			var stripeEvent = EventUtility.ConstructEvent(
				json,
				Request.Headers["Stripe-Signature"],
				_stripeSettings.WebhookSecret
			);

			// Sadece "checkout.session.completed" event'ini işleyelim
			if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
			{
				var session = stripeEvent.Data.Object as Session;

				if (session != null && session.PaymentStatus == "paid")
				{
					// Metadata üzerinden gerekli bilgileri alın
					var orderId = int.Parse(session.Metadata["OrderId"]);
					var marketReceiverId = session.Metadata["MarketReceiverId"];

					if (Guid.TryParse(marketReceiverId, out var marketReceiverGuid))
					{
						// Sipariş durumunu güncelleyin
						await _productService.UpdateOrderStatus(orderId, "Paid");

						var orderItems = await _cartService.GetOrderItemsByOrderIdAsync(orderId);
						foreach (var item in orderItems)
						{
							await _productService.UpdateProductQuantity(item.ProductId, item.Quantity);
						}

						// Sepeti temizleyin
						await _cartService.ClearCartAsync(marketReceiverGuid);

						// Ürünlerin miktarını güncelleyin
						

						return Ok("Ödeme başarılı.");
					}
				}
			}

			return BadRequest("İşlenemeyen Stripe event'i.");
		}
		catch (StripeException ex)
		{
			return BadRequest($"Stripe hatası: {ex.Message}");
		}
	}

	[HttpGet("GetOrderStatus/{orderId}")]
	public async Task<IActionResult> GetOrderStatus(int orderId)
	{
		var order = await _productService.GetOrderByIdAsync(orderId);
		if (order == null)
			return NotFound("Sipariş bulunamadı.");

		return Ok(new { order.Status });
	}
}
