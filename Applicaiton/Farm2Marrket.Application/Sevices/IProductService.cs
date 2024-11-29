using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Sevices
{
    public interface IProductService
    {
        Task<ProductResponseDto> AddProduct(Guid farmerId, ProductDto productDto);
        Task DeleteProductAsync(int productId);
        Task<bool> UpdateProductQuantity(int id, int amount);

		Task<IEnumerable<ProductDto>> GetProductsByFarmerIdAsync(Guid farmerId);

    }
}
