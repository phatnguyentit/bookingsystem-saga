using BookingSystem.BookingService.Application.Interfaces;
using BookingSystem.Shared.Contracts.DTOs;
using System.Net.Http.Json;

namespace BookingSystem.BookingService.Infrastructure.HttpClients;

public class CatalogServiceClient(HttpClient http) : ICatalogServiceClient
{
    public Task<ListingDto?> GetListingAsync(Guid listingId, CancellationToken cancellationToken = default)
        => http.GetFromJsonAsync<ListingDto>($"/api/catalog/listings/{listingId}", cancellationToken);
}
