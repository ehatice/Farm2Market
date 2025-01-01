using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
	public class OrderItem
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public string ProductName { get; set; } // Ürünün adı


		[ForeignKey("OrderId")]
		public Order Order { get; set; }

		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}
}
