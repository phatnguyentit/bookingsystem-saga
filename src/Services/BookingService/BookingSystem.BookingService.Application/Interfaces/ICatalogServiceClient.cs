using BookingSystem.Shared.Contracts.DTOs;

namespace BookingSystem.BookingService.Application.Interfaces;

public interface ICatalogServiceClient
{
    Task<ListingDto?> GetListingAsync(Guid listingId, CancellationToken cancellationToken = default);
}
