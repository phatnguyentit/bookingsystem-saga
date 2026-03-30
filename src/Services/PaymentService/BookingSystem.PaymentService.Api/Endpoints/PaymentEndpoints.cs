using BookingSystem.PaymentService.Api.Features.ProcessPayment;
using BookingSystem.PaymentService.Infrastructure.Persistence;
using MediatR;

namespace BookingSystem.PaymentService.Api.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments").RequireAuthorization();

        group.MapPost("/", async (ProcessPaymentCommand cmd, ISender sender) =>
        {
            var id = await sender.Send(cmd);
            return Results.Created($"/api/payments/{id}", new { id });
        })
        .WithName("ProcessPayment");

        group.MapGet("/{id:guid}", async (Guid id, IPaymentRepository repo) =>
        {
            var payment = await repo.GetByIdAsync(id);
            return payment is null ? Results.NotFound() : Results.Ok(payment);
        })
        .WithName("GetPayment");
    }
}
