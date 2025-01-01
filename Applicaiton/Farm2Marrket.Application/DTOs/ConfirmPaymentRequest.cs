using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
	public class ConfirmPaymentRequest
	{
		public int OrderId { get; set; }
		public string SessionId { get; set; }
		public String MarketReceiverId { get; set; }
	}
}
