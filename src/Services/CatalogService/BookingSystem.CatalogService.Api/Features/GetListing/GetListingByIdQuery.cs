using BookingSystem.CatalogService.Infrastructure.Repositories;
using BookingSystem.Shared.Contracts.DTOs;
using MediatR;

namespace BookingSystem.CatalogService.Api.Features.GetListing;

public record GetListingByIdQuery(Guid ListingId) : IRequest<ListingDto?>;

public class GetListingByIdHandler(IListingRepository repo)
    : IRequestHandler<GetListingByIdQuery, ListingDto?>
{
    public async Task<ListingDto?> Handle(GetListingByIdQuery q, CancellationToken cancellationToken)
    {
        var listing = await repo.GetByIdAsync(q.ListingId, cancellationToken);
        return listing is null ? null
            : new ListingDto(
                listing.Id,
                listing.Title,
                listing.Description,
                listing.PricePerNight,
                listing.Currency,
                listing.IsAvailable);
    }
}
