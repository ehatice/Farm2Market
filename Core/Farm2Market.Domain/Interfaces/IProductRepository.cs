using Farm2Market.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Interfaces
{
    public interface IProductRepository
    {

		IQueryable<Product> Products { get; }
		IQueryable<Category> Categories { get; }
		Task AddAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<Product> GetByIdAsync(int id);
		Task UpdateProductQuantity(int id, int newQuantity);
		Task<IEnumerable<Product>> GetProductsByFarmerIdAsync(Guid farmerId);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetProductss(int productId);
        Task UpdateAsync(Product product);
        Task<Product> GetProducts1(int productId);


        Task<Category> GetCategoryByIdAsync(int id);
		Task AddCategoryAsync(Category category);

		Task<IEnumerable<Category>> GetCategory();
		Task<Order> GetOrderByIdAsync(int orderId);
		Task UpdateAsync(Order order);


	}
}
