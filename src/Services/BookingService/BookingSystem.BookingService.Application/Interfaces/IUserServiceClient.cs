namespace BookingSystem.BookingService.Application.Interfaces;

public interface IUserServiceClient
{
    Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}
