namespace BookingSystem.Shared.Contracts.Events;

public record PaymentSucceededIntegrationEvent(
    Guid PaymentId,
    Guid BookingId,
    Guid UserId,
    decimal Amount,
    string Currency,
    DateTime OccurredAt);
