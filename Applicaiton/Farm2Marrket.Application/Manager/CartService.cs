using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddToCart(Guid marketReceiverId, AddToCartDto model)
        {
            
            var product = await _productRepository.GetProductss(model.ProdcutId);
            if (product == null)
                throw new Exception("Ürün bulunamadı.");

            var cart = await _cartRepository.GetCartWithItemsAsync(marketReceiverId);

            if (cart == null)
            {
              
                cart = new Cart
                {
                    MarketReceiverId = marketReceiverId.ToString(),
                    CartItems = new List<CartItem>(),
                    TotalPrice = 0
                };
                await _cartRepository.AddCartAsync(cart);
            }

            
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == model.ProdcutId);
            if (cartItem != null)
            {
                
                cartItem.Quantity += model.WeightOrAmount;
            }
            else
            {
                
                cart.CartItems.Add(new CartItem
                {
                    ProductId = model.ProdcutId,
                    Quantity = model.WeightOrAmount,
                   
                });
            }

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);

           
            await _cartRepository.SaveChangesAsync();
        }
    
    }
}
