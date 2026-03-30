using BookingSystem.SearchService.Api.Features.SearchListings;
using MediatR;

namespace BookingSystem.SearchService.Api.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/search");

        group.MapGet("/listings", async (
            string? query,
            DateOnly? checkIn,
            DateOnly? checkOut,
            decimal? maxPrice,
            int page,
            int pageSize,
            ISender sender) =>
        {
            var results = await sender.Send(new SearchListingsQuery(
                query, checkIn, checkOut, maxPrice, page, pageSize));
            return Results.Ok(results);
        })
        .WithName("SearchListings");
    }
}
