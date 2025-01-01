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
			if (order == null)
			{
				throw new ArgumentNullException(nameof(order), "Order cannot be null.");
			}

			try
			{
				await _appDbContext.Orders.AddAsync(order);
				await _appDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while adding the order: {ex.Message}", ex);
			}
		}

		public async Task UpdateAsync(Cart cart)
		{
			if (cart == null)
			{
				throw new ArgumentNullException(nameof(cart), "Cart cannot be null.");
			}

			try
			{
				_appDbContext.Carts.Update(cart);
				await _appDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating the cart: {ex.Message}", ex);
			}
		}

		public async Task<Order> GetPendingOrderForUserAsync(string userId)
		{
			return await _appDbContext.Orders
				.Include(o => o.OrderItems)  // Sipariş detaylarını da dahil et
				.FirstOrDefaultAsync(o => o.MarketReceiverId == userId && o.Status == "Pending");
		}

		public async Task<Order> GetOrderByIdAsync(int orderId)
		{
			return await _appDbContext.Orders
				.Include(o => o.OrderItems) // OrderItems'ı dahil ediyoruz
				.FirstOrDefaultAsync(o => o.Id == orderId);
		}


	}
}
