using BookingSystem.CatalogService.Infrastructure.Persistence;

namespace BookingSystem.CatalogService.Infrastructure.Repositories;

public interface IListingRepository
{
    Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Listing listing, CancellationToken cancellationToken = default);
}
