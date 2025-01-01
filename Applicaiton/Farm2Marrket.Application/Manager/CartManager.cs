using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Sevices;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
    public class CartManager : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartManager(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddToCart(Guid marketReceiverId, AddToCartDto model)
        {

            var product = await _productRepository.GetProducts1(model.ProductId);
            if (product == null)
                throw new Exception("Ürün bulunamadı.");

            var cart = await _cartRepository.GetCartWithItemsAsync(marketReceiverId);

            if (cart == null)
            {

                cart = new Cart
                {
                    MarketReceiverId = marketReceiverId.ToString(),
                    CartItems = new List<CartItem>(),
                    TotalPrice = 0,
                };
                await _cartRepository.AddCartAsync(cart);
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == model.ProductId);
            if (cartItem != null)
            {
                cartItem.WeightOrAmount += model.WeightOrAmount;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = model.ProductId,
                    ProductName = product.Name,
                    WeightOrAmount = model.WeightOrAmount,
                    Price=product.Price,
                    
                });
            }

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.WeightOrAmount * ci.Price);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<CartDto> GetCartAsync(Guid marketReceiverId)
        {
            var cart = await _cartRepository.GetCartAsync(marketReceiverId);

            if (cart == null)
            {
                return null;
            }

            return new CartDto
            {
                CartId = cart.CartId,
                TotalPrice = cart.TotalPrice,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    WeightOrAmount = ci.WeightOrAmount,
                    UnitPrice = ci.Product.Price,
                    TotalPrice = ci.WeightOrAmount * ci.Product.Price
                }).ToList()
            };

        }
        public async Task RemoveCartItemAsync(int cartItemId,Guid marketReceiverId)
        {
            
            await _cartRepository.RemoveCartItemAsync(cartItemId);
            var cart = await _cartRepository.GetCartWithItemsAsync(marketReceiverId);
            cart.TotalPrice = cart.CartItems.Sum(ci => ci.WeightOrAmount * ci.Price);
            await _cartRepository.SaveChangesAsync();
        }





        public async Task<OrderDto> CreateOrderFromCartAsync(Guid marketReceiverId)
        {
            // Sepeti al
            var cart = await _cartRepository.GetCartByMarketReceiverIdAsync(marketReceiverId);
            

            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("The cart is empty.");
            }

            // Siparişi oluştur
            var order = new Order
            {
                MarketReceiverId = cart.MarketReceiverId,
                TotalPrice = cart.TotalPrice,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.WeightOrAmount,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    
                }).ToList()
            };

            // Siparişi kaydet
            await _cartRepository.AddAsync(order);

            await _cartRepository.UpdateAsync(cart);

            // DTO'yu döndür
            return new OrderDto
            {
                OrderId = order.Id,
                MarketReceiverId = order.MarketReceiverId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Total = oi.Quantity * oi.Price
                }).ToList()
            };




        }

		public async Task ClearCartAsync(Guid marketReceiverId)
		{
			// Kullanıcının sepetini al
			var cart = await _cartRepository.GetCartWithItemsAsync(marketReceiverId);

			if (cart != null)
			{
				// Sepetteki ürünleri temizle
				cart.CartItems.Clear();

				// Sepetin toplam fiyatını sıfırla
				cart.TotalPrice = 0;

				// Değişiklikleri kaydet
				await _cartRepository.SaveChangesAsync();
			}
			else
			{
				throw new Exception("Kullanıcıya ait sepet bulunamadı.");
			}
		}


		public async Task<Order> GetPendingOrderForUserAsync(string userId)
		{
			var order = await _cartRepository.GetPendingOrderForUserAsync(userId);

			if (order == null)
			{
				throw new InvalidOperationException("Pending order not found for the user.");
			}

			return order;
		}


		public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
		{
			var order = await _cartRepository.GetOrderByIdAsync(orderId);
			if (order == null)
				return null;

			return order.OrderItems.ToList();
		}


	}
}
