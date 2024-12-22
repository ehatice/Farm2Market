using Farm2Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Sevices
{
    public interface IMarketFavoriteService
    {

        Task AddFavoriteAsync(Guid marketReceiverId, int productId);
        Task RemoveFavoriteAsync(Guid marketReceiverId, int productId);
        Task<List<Product>> GetFavoritesByMarketAsync(Guid marketReceiverId);
    }
}
