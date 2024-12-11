using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Farm2Market.Infrastructure.EntityTypeConfig
{
    public class CartConfig: BaseEnitityConfig<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasMany(c => c.CartItems)
                   .WithOne(ci => ci.Cart)
                   .HasForeignKey(ci => ci.CartId);


           // builder.HasOne(c => c.MarketReceiver) 
           //   .WithOne(mr => mr.Cart)        
            //  .HasForeignKey<Cart>(c => c.MarketReceiverId)
            //  .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
