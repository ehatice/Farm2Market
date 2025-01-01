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
                var cartItem = await _appDbContext.CartItems.Include(ci => ci.Cart).FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

                if (cartItem == null)
                    throw new Exception("Cart item not found.");

                if (cartItem.Cart != null)
                {
                    cartItem.Cart.TotalPrice -= cartItem.Price;
                    cartItem.Cart.TotalPrice = Math.Max(0, cartItem.Cart.TotalPrice); // Negatif değerleri önle
                }
                _appDbContext.CartItems.Remove(cartItem);

                await _appDbContext.SaveChangesAsync();
            

        }

        //public async Task<Cart> GetCartByIdAsync(int cartId)
        //{
        //  var cart = await _appDbContext.Carts
        //    .Include(c => c.CartItems)
        //  .ThenInclude(ci => ci.Product)
        //  .FirstOrDefaultAsync(c => c.CartId == cartId);

        //if (cart != null)
        //{
        //    cart.CalculateTotalPrice();
        //}

        //return cart;
        //}








		public async Task<Cart> GetCartByMarketReceiverIdAsync(Guid marketReceiverId)
		{
			return await _appDbContext.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.MarketReceiverId == marketReceiverId.ToString());
		}

		public async Task AddAsync(Order order)
		{
			await _appDbContext.Orders.AddAsync(order);
			await _appDbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(Cart cart)
		{
			_appDbContext.Carts.Update(cart);
			await _appDbContext.SaveChangesAsync();
		}

	}
}
