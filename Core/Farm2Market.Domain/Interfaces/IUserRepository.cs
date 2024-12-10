using Farm2Market.Domain.Entities;

namespace Farm2Market.Domain
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<int> GetConfirmNumber(string id);
        Task<bool> GetConfirmedEmail(string id);
		Task<AppUser> GetByIdAsync(string id);

	}
}
