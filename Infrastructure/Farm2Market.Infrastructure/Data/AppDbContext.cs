using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Farm2Market.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }
        public AppDbContext()
        {
            
        }
        // Veritabanı tablolarını temsil eden DbSet'ler
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=farm2markett", new MySqlServerVersion(new Version(9, 0, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API ile tablo ayarları yapılabilir
            modelBuilder.Entity<User>().ToTable("Users");
            base.OnModelCreating(modelBuilder);
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
