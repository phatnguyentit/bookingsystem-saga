namespace BookingSystem.Shared.Contracts.DTOs;

public record BookingDto(
    Guid Id,
    Guid UserId,
    Guid ListingId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    decimal Amount,
    string Currency,
    string Status);
