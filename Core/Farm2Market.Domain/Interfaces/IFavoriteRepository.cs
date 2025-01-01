using Farm2Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(Guid MarketReceiverId, int productId);
        Task RemoveFavoriteAsync(Guid MarketReceiverId, int productId);
        Task<List<Product>> GetFavoritesByMarketAsync(Guid MarketReceiverId);
        Task<bool> IsProductFavoritedAsync(Guid marketReceiverId, int productId);
    }
}
