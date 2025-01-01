using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int WeightOrAmount { get; set; }

        public decimal Price { get; set; }
        public Cart Cart { get; set;}
        public Product Product { get; set; }


    }
}
