
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class Product: IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WeightOrAmount { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        //public ProductCategory Category { get; set; }
        public string Quality { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
		public byte[]? Image1 { get; set; }
		public byte[]? Image2 { get; set; }
		public byte[]? Image3 { get; set; }
		public string UnitType { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Farmer  Farmer { get; set; }
        public Guid FarmerId { get; set; }

		public int CategoryId { get; set; } // Foreign Key
		public Category Category { get; set; }

		public ICollection<CartItem> CartItems { get; set; }

        public ICollection<MarketFavorite> FavoritedByMarkets { get; set; }
    }   
}