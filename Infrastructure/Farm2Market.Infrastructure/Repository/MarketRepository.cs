using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Market.Infrastructure.Data;
using Farm2Market.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Infrastructure.Repository
{
    public class MarketRepository : IMarketRepository
    {
        private readonly AppDbContext _context;

        public MarketRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MarketReceiver> GetByIdAsync(Guid marketId)
        {
            string marketIdString = marketId.ToString();
            return await _context.MarketReceivers.FirstOrDefaultAsync(f => f.Id == marketIdString);
        }

        public async Task<bool> UpdateAsync(MarketReceiver market)
        {
            _context.MarketReceivers.Update(market);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
