using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
	public class CreateCheckoutSessionRequest
	{
		public int OrderId { get; set; }
		public string SuccessUrl { get; set; }
		public string CancelUrl { get; set; }
	}
}
