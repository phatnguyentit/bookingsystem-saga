using BookingSystem.CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.CatalogService.Infrastructure.Repositories;

public class ListingRepository(CatalogDbContext db) : IListingRepository
{
    public Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.Listings.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task AddAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        await db.Listings.AddAsync(listing, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
