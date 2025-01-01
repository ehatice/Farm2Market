using Farm2Marrket.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe.Climate;
using Stripe;
using Farm2Marrket.Application.Sevices;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly StripeSettings _stripeSettings;
	private readonly IProductService _productService;

	public PaymentController(IOptions<StripeSettings> stripeSettings, IProductService orderService)
	{
		_stripeSettings = stripeSettings.Value;
		_productService = orderService;
	}

	[HttpPost("CreateCheckoutSession")]
	public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
	{
		var order = await _productService.GetOrderByIdAsync(request.OrderId);
		if (order == null || order.Status != "Pending")
			return BadRequest("Geçersiz sipariş.");

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
					Description = item.Description
				}
			},
			Quantity = item.Quantity
		}).ToList();

		var options = new SessionCreateOptions
		{
			PaymentMethodTypes = new List<string> { "card" },
			LineItems = lineItems,
			Mode = "payment",
			SuccessUrl = request.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
			CancelUrl = request.CancelUrl
		};

		var service = new SessionService();
		var session = service.Create(options);

		return Ok(new { sessionId = session.Id, url = session.Url });
	}

	[HttpPost("ConfirmPayment")]
	public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
	{
		StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

		var service = new SessionService();
		var session = service.Get(request.SessionId);

		if (session.PaymentStatus == "paid")
		{
			await _productService.UpdateOrderStatus(request.OrderId, "Paid");
			return Ok("Ödeme başarılı.");
		}

		return BadRequest("Ödeme tamamlanmadı.");
	}
}
