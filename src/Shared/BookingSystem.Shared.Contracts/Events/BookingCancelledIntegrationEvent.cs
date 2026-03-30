namespace BookingSystem.Shared.Contracts.Events;

public record BookingCancelledIntegrationEvent(
    Guid BookingId,
    Guid UserId,
    Guid ListingId,
    string Reason,
    DateTime OccurredAt);
