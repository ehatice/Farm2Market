using Farm2Market.Domain.Entities;
using Farm2Market.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Infrastructure.EntityTypeConfig
{
    public class MarketConfig : BaseEnitityConfig<MarketReceiver>
    {
        public void Configure(EntityTypeBuilder<MarketReceiver> builder)
        {
            builder.HasMany(m => m.Favorites)
                   .WithOne(f => f.MarketReceiver)
                   .HasForeignKey(f => f.MarketReceiverId);
        }
    }
}
