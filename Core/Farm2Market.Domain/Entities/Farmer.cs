using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class Farmer:AppUser
    {
        public string Adress { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
