using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        protected readonly AppDbContext _appDbContext;
        private DbSet<Product> _product;
        public ProductRepository(AppDbContext context)
        {
            _appDbContext = context;
            _product = _appDbContext.Set<Product>();
        }
        public async Task AddAsync(Product product)
        {
            _product.Add(product);
            _appDbContext.SaveChanges();
        }
        public async Task DeleteProductAsync(Product product)
        {
            product.IsActive = true;  // Soft delete: ürünü pasif yap
            product.DeletedDate = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerIdAsync(Guid farmerId)
        {
            return await _appDbContext.Products
                .Where(p => p.FarmerId == farmerId && p.IsActive)
                .ToListAsync();
        }
    }
}
