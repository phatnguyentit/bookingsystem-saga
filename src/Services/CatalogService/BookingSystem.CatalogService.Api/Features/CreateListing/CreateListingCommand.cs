using BookingSystem.CatalogService.Infrastructure.Persistence;
using BookingSystem.CatalogService.Infrastructure.Repositories;
using MediatR;

namespace BookingSystem.CatalogService.Api.Features.CreateListing;

public record CreateListingCommand(
    string Title,
    string Description,
    decimal PricePerNight,
    string Currency) : IRequest<Guid>;

public class CreateListingHandler(IListingRepository repo)
    : IRequestHandler<CreateListingCommand, Guid>
{
    public async Task<Guid> Handle(CreateListingCommand cmd, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = cmd.Title,
            Description = cmd.Description,
            PricePerNight = cmd.PricePerNight,
            Currency = cmd.Currency,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };
        await repo.AddAsync(listing, cancellationToken);
        return listing.Id;
    }
}
