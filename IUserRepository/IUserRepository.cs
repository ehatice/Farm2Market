using Farm2Market.Domain;

namespace IUserRepository
{
    public interface IAppUserRepository
    {
        Task AddAsync(User user);

    }
}
