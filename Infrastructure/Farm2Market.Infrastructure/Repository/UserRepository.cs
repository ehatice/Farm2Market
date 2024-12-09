using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain;
using Farm2Market.Domain.Entities;
using Farm2Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Farm2Market.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly AppDbContext _appDbContext;
        private DbSet<User> _users;
		private DbSet<Farmer> _farmers;
		public UserRepository( AppDbContext context) {
            _appDbContext = context;
            _users = _appDbContext.Set<User>();
            _farmers = _appDbContext.Set<Farmer>();
        }
        public async Task AddAsync(User user)
        {
            _users.Add(user);
            _appDbContext.SaveChanges();
        }


		public async Task UpdateAsync(Product product)
		{
			_appDbContext.Entry(product).State = EntityState.Modified;
			await _appDbContext.SaveChangesAsync();
		}

		public async Task<int> GetConfirmNumber(string id)
        {
			var farmer = await _farmers.FirstOrDefaultAsync(x => x.Id == id);

			if (farmer == null)
			{
				//Farmer bulunamadıysa burada bir işlem yapabilirsiniz
				Console.WriteLine("Farmer not found.");
				return 1;
			}
			else
			{
				//Farmer bulunduğunda burada işlem yapabilirsiniz
				Console.WriteLine($"Farmer ID: {farmer.Id}");
				return farmer.ConfirmationNumber;
			}
		}
		public async Task<bool> GetConfirmedEmail(string id)
		{
			var entity = await _farmers.FirstOrDefaultAsync(x => x.Id == id);
			if (entity != null)
			{
				entity.EmailConfirmed = true;
				
				_farmers.Entry(entity).State = EntityState.Modified;
				await _appDbContext.SaveChangesAsync();
				return true;
			}
			else { return false; }
		}
	}
}
