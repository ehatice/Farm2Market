using Farm2Market.Domain.Enums;
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
        public ProductCategory Category { get; set; }
        public string Quality { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; } 
        public string UnitType { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Farmer  Farmer { get; set; }
        public Guid FarmerId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}


