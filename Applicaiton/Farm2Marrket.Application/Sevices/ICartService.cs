﻿using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Sevices
{
    public interface ICartService
    {
        Task AddToCart(Guid marketReceiverId, AddToCartDto model);

        Task<CartDto> GetCartAsync(Guid marketReceiverId);
		Task<OrderDto> CreateOrderFromCartAsync(Guid marketReceiverId);

	
        Task RemoveCartItemAsync(int cartItemId, Guid marketReceiverId);
        Task ClearCartAsync(Guid marketReceiverId);
		Task<Order> GetPendingOrderForUserAsync(string userId);
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
	}
}
