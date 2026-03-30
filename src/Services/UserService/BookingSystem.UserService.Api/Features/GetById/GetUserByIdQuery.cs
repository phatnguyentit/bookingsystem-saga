using BookingSystem.UserService.Infrastructure.Repositories;
using MediatR;

namespace BookingSystem.UserService.Api.Features.GetById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;

public record UserDto(Guid Id, string Email, string FullName, DateTime CreatedAt);

public class GetUserByIdHandler(IUserRepository repo)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery q, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(q.UserId, cancellationToken);
        return user is null ? null
            : new UserDto(user.Id, user.Email, user.FullName, user.CreatedAt);
    }
}
