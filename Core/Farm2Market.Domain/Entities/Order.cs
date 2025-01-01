using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public string MarketReceiverId { get; set; }
		public string Status { get; set; } = "Pending";
		public decimal TotalPrice { get; set; }
		public DateTime OrderDate { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

		[ForeignKey("MarketReceiverId")]
		public MarketReceiver MarketReceiver { get; set; }
	}
}
