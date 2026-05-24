namespace BookingSystem.BookingService.Application.Interfaces.UoW;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
