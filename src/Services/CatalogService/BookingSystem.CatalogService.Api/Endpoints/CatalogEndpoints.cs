using BookingSystem.CatalogService.Api.Features.GetListing;
using BookingSystem.CatalogService.Api.Features.CreateListing;
using MediatR;

namespace BookingSystem.CatalogService.Api.Endpoints;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/catalog");

        group.MapGet("/listings/{id:guid}", async (Guid id, ISender sender) =>
        {
            var listing = await sender.Send(new GetListingByIdQuery(id));
            return listing is null ? Results.NotFound() : Results.Ok(listing);
        })
        .WithName("GetListing");

        group.MapPost("/listings", async (CreateListingCommand cmd, ISender sender) =>
        {
            var id = await sender.Send(cmd);
            return Results.Created($"/api/catalog/listings/{id}", new { id });
        })
        .RequireAuthorization()
        .WithName("CreateListing");
    }
}
