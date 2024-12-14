﻿using Farm2Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartWithItemsAsync(Guid marketReceiverId);
        Task AddCartAsync(Cart cart);
        Task SaveChangesAsync();


    }
}