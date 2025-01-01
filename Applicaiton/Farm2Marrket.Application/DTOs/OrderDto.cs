using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
	public class OrderDto
	{
		public int OrderId { get; set; }
		public string MarketReceiverId { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime OrderDate { get; set; }
		public List<OrderItemDto> Items { get; set; }
	}
}
