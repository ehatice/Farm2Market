using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
	public class Category
	{

		public int Id { get; set; } // Primary Key
		public string Name { get; set; } // Category name (e.g., "Electronics", "Books")

		// Navigation Property for related Products
		public ICollection<Product> Products { get; set; }

	}
}
