using Farm2Market.Domain.Entities;

namespace Farm2Market.Domain
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

    }
}
