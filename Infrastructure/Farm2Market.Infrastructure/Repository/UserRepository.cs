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
        public UserRepository( AppDbContext context) {
            _appDbContext = context;
            _users = _appDbContext.Set<User>();
        }
        public async Task AddAsync(User user)
        {
            _users.Add(user);
            _appDbContext.SaveChanges();
        }
    }
}
