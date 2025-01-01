using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
	public class StripeSettings
	{
		public string SecretKey { get; set; }
		public string PublicKey { get; set; }
		public string WebhookSecret { get; set; }
	}
}
