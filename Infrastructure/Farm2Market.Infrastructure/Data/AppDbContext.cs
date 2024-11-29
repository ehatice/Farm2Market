using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain.Entities;
using Farm2Market.Infrastructure.EntityTypeConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Farm2Market.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext<AppUser>
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }
        public AppDbContext()
        {}

   
        public DbSet<User> Users { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<MarketReceiver> MarketReceivers { get; set; }
        public DbSet<Product> Products { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=08080808;database=farm2markett", new MySqlServerVersion(new Version(9, 0, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

     
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Farmer>().ToTable("Farmers");
            modelBuilder.Entity<MarketReceiver>().ToTable("MarketReceivers");
            modelBuilder.Entity<Product>().ToTable("Products");
      



            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin",
                    Email = "admin",
                });
        }
    }
}
