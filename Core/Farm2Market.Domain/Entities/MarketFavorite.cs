using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class MarketFavorite
    {
        public int Id { get; set; }
        public Guid MarketReceiverId { get; set; }
        public int ProductId { get; set; }

        public MarketReceiver MarketReceiver { get; set; }
        public Product Product { get; set; }
    }
}
