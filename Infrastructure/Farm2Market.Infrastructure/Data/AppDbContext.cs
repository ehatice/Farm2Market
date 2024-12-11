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

		public DbSet<Category> Categories { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart>Carts { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=farm2markett", new MySqlServerVersion(new Version(9, 0, 0)));
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

     
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Farmer>().ToTable("Farmers");
            modelBuilder.Entity<MarketReceiver>().ToTable("MarketReceivers");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<Cart>().ToTable("Carts");




            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new CartConfig());
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin",
                    Email = "admin",
                });
			modelBuilder.Entity<Product>()
			.HasOne(p => p.Category)
			.WithMany(c => c.Products)
			.HasForeignKey(p => p.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);
		}






    }
}
