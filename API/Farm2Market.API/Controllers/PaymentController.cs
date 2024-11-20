﻿using Farm2Marrket.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.V2;

namespace Farm2Market.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly StripeSettings _stripeSettings;

		public string SessionId { get; set; }
		public PaymentController(IOptions<StripeSettings> stripeSettings)
		{
			_stripeSettings = stripeSettings.Value;

		}

		[HttpPost]
		public IActionResult CreateCheckoutSession(string amount)
		{

			var currency = "usd"; 
			var successUrl = "https://localhost:44342/Home/success";
			var cancelUrl = "https://localhost:44342/Home/cancel";
			StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
					"card"
				},
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = currency,
							UnitAmount = Convert.ToInt32(amount) * 100,  
                            ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "Product Name",
								Description = "Product Description"
							}
						},
						Quantity = 1
					}
				},
				Mode = "payment",
				SuccessUrl = successUrl,
				CancelUrl = cancelUrl
			};

			var service = new SessionService();
			var session = service.Create(options);
			SessionId = session.Id;

			return Ok(session.Url);
		}

	}
}

