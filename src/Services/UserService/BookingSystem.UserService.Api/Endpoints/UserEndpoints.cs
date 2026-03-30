using BookingSystem.UserService.Api.Features.Create;
using BookingSystem.UserService.Api.Features.GetById;
using MediatR;

namespace BookingSystem.UserService.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var user = await sender.Send(new GetUserByIdQuery(id));
            return user is null ? Results.NotFound() : Results.Ok(user);
        })
        .WithName("GetUser");

        group.MapPost("/", async (CreateUserCommand cmd, ISender sender) =>
        {
            var id = await sender.Send(cmd);
            return Results.Created($"/api/users/{id}", new { id });
        })
        .WithName("CreateUser");
    }
}
