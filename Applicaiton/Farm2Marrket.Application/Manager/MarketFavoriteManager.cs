using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Marrket.Application.Sevices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
    public class MarketFavoriteManager : IMarketFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public MarketFavoriteManager(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task AddFavoriteAsync(Guid marketReceiverId, int productId)
        {
            if (marketReceiverId == Guid.Empty)
            {
                throw new ArgumentException("Invalid market receiver ID.");
            }

            var isAlreadyFavorited = await _favoriteRepository.IsProductFavoritedAsync(marketReceiverId, productId);
            if (isAlreadyFavorited)
            {
                throw new InvalidOperationException("This product is already in your favorites.");
            }

            await _favoriteRepository.AddFavoriteAsync(marketReceiverId, productId);
        }

        public async Task RemoveFavoriteAsync(Guid marketReceiverId, int productId)
        {
            if (marketReceiverId == Guid.Empty)
            {
                throw new ArgumentException("Invalid market receiver ID.");
            }

            await _favoriteRepository.RemoveFavoriteAsync(marketReceiverId, productId);
        }

        public async Task<List<Product>> GetFavoritesByMarketAsync(Guid marketReceiverId)
        {
  
            if (marketReceiverId == Guid.Empty)
            {
                throw new ArgumentException("Invalid market receiver ID.");
            }

            return await _favoriteRepository.GetFavoritesByMarketAsync(marketReceiverId);
        }

    }

}
