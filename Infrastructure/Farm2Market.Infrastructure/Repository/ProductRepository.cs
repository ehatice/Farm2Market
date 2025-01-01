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
            product.IsActive = true; 
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
                .Where(p => p.FarmerId == farmerId && !p.IsActive)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _appDbContext.Products
                .Where(p => !p.IsActive)
                .ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetProductss(int productId) //Sepet için id ile Product alıyoruz.
        {
            return await _appDbContext.Products
                .Where(p => p.Id == productId && !p.IsActive)
                .ToListAsync();
        }

        public async Task<Product> GetProducts1(int productId) //Sepet için id ile Product alıyoruz.
        {
            return await _appDbContext.Products.FirstOrDefaultAsync(p => p .Id == productId && p.IsActive== false);
        }


        public async Task UpdateProductQuantity(int id, int newQuantity)
		{
			var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				throw new Exception("Product bulunamadı");
			}

			product.WeightOrAmount = newQuantity;
			_appDbContext.Products.Update(product);
			await _appDbContext.SaveChangesAsync();
		}


		public async Task UpdateAsync(Product product)
		{
			_appDbContext.Products.Update(product);
			await _appDbContext.SaveChangesAsync();
		}
		public IQueryable<Product> Products => _appDbContext.Products;
		public IQueryable<Category> Categories => _appDbContext.Categories; // Kategoriler için ekleme



		public async Task<Category> GetCategoryByIdAsync(int id)
		{
			return await _appDbContext.Categories.FindAsync(id);
		}


		public async Task AddCategoryAsync(Category category)
		{
			await _appDbContext.Categories.AddAsync(category);
			await _appDbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<Category>> GetCategory()
		{
			return await _appDbContext.Categories
		.Select(c => new Category
		{
			Id = c.Id,
			Name = c.Name
		})
		.ToListAsync();
		}
	}
}
