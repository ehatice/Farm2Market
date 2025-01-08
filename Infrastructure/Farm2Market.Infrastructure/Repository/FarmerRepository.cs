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
    public class FarmerRepository : IFarmerRepository
    {

        private readonly AppDbContext _context;

        public FarmerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Farmer> GetByIdAsync(Guid farmerId)
        {
            string farmerIdString = farmerId.ToString();
            return await _context.Farmers.FirstOrDefaultAsync(f => f.Id == farmerIdString);
        }

        public async Task<bool> UpdateAsync(Farmer farmer)
        {
            _context.Farmers.Update(farmer);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
