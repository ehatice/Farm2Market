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
    public class FavoriteRepository : IFavoriteRepository
    {
        protected readonly AppDbContext _appDbContext;
        

        public FavoriteRepository(AppDbContext context)
        {
            _appDbContext = context;
        }
        public async Task<bool> IsProductFavoritedAsync(Guid marketReceiverId, int productId)
        {
            return await _appDbContext.MarketFavorites
                .AnyAsync(f => f.MarketReceiverId == marketReceiverId && f.ProductId == productId);
        }

        public async Task AddFavoriteAsync(Guid marketReceiverId, int productId)
        {
            var favorite = new MarketFavorite
            {
                MarketReceiverId = marketReceiverId,
                ProductId = productId,
            };

            await _appDbContext.MarketFavorites.AddAsync(favorite);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(Guid marketReceiverId, int productId)
        {
            var favorite = await _appDbContext.MarketFavorites
                .FirstOrDefaultAsync(f => f.MarketReceiverId == marketReceiverId && f.ProductId == productId);

            if (favorite != null)
            {
                _appDbContext.MarketFavorites.Remove(favorite);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetFavoritesByMarketAsync(Guid marketReceiverId)
        {
            return await _appDbContext.MarketFavorites
                .Where(f => f.MarketReceiverId == marketReceiverId)
                .Select(f => f.Product)
                .ToListAsync();
        }
    }

}
