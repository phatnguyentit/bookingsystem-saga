using BookingSystem.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.UserService.Infrastructure.Repositories;

public class UserRepository(UserDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await db.Users.AddAsync(user, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
