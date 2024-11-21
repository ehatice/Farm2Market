using Farm2Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByFarmerIdAsync(Guid farmerId);
    }
}
