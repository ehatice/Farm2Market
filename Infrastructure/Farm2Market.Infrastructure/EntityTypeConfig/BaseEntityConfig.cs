using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain.Entities;

namespace Farm2Market.Infrastructure.EntityTypeConfig
{
    public class BaseEnitityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                   .HasColumnType("datetime");

            builder.Property(e => e.UpdatedDate)
                   .HasColumnType("datetime");

            builder.Property(e => e.DeletedDate)
                   .HasColumnType("datetime");
        }
    }
}
