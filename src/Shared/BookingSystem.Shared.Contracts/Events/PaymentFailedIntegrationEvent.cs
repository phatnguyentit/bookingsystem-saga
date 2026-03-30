namespace BookingSystem.Shared.Contracts.Events;

public record PaymentFailedIntegrationEvent(
    Guid PaymentId,
    Guid BookingId,
    Guid UserId,
    string Reason,
    DateTime OccurredAt);
