﻿using Farm2Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Farm2Market.Infrastructure.EntityTypeConfig
{
    public class ProductConfig : BaseEnitityConfig<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasOne(a => a.Farmer)
            .WithMany(u =>u.Products)
            .HasForeignKey(a => a.FarmerId);


            builder.HasMany(p => p.CartItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            builder.HasMany(p => p.FavoritedByMarkets)
            .WithOne(f => f.Product)
            .HasForeignKey(f => f.ProductId);


        }
    }
}
