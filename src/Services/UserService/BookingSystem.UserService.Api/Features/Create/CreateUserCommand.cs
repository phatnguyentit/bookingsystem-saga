using BookingSystem.UserService.Infrastructure.Persistence;
using BookingSystem.UserService.Infrastructure.Repositories;
using MediatR;

namespace BookingSystem.UserService.Api.Features.Create;

public record CreateUserCommand(string Email, string FullName, string PasswordHash) : IRequest<Guid>;

public class CreateUserHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand cmd, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = cmd.Email,
            FullName = cmd.FullName,
            PasswordHash = cmd.PasswordHash,
            CreatedAt = DateTime.UtcNow
        };
        await repo.AddAsync(user, cancellationToken);
        return user.Id;
    }
}
