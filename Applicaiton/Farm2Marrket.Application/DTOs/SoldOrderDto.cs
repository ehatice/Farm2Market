using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
	public class SoldOrderDto
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public string MarketReceiverName { get; set; } // Siparişi alan MarketReceiver
		public List<SoldProductDto> Products { get; set; }
	}
}
