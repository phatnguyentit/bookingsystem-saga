using BookingSystem.UserService.Infrastructure.Persistence;

namespace BookingSystem.UserService.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
