using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class Cart: IBaseEntity
    {
        public int CartId { get; set; }
        public String MarketReceiverId {  get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        [ForeignKey("MarketReceiverId")]
        public MarketReceiver MarketReceiver { get; set; }
    }
}
