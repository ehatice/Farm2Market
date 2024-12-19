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
    public class CartRepository : ICartRepository
    {
        protected readonly AppDbContext _appDbContext;
        private DbSet<Cart> _cart;
        public CartRepository(AppDbContext context)
        {
            _appDbContext = context;
        }
        public async Task<Cart> GetCartWithItemsAsync(Guid marketReceiverId)
        {
            return await _appDbContext.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.MarketReceiverId == marketReceiverId.ToString());
        }
        public async Task AddCartAsync(Cart cart)
        {
            await _appDbContext.Carts.AddAsync(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Cart> GetCartAsync(Guid marketReceiverId)
        {
            return await _appDbContext.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.MarketReceiverId == marketReceiverId.ToString() && c.IsActive);
        }



        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _appDbContext.CartItems.FindAsync(cartItemId);

            if (cartItem != null)
            {
                _appDbContext.CartItems.Remove(cartItem);
                await _appDbContext.SaveChangesAsync();
            }
        }


    }
}
