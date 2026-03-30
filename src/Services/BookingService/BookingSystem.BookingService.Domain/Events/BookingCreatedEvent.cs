using BookingSystem.BookingService.Domain.Common;
using BookingSystem.BookingService.Domain.ValueObjects;

namespace BookingSystem.BookingService.Domain.Events;

public record BookingCreatedEvent(
    BookingId BookingId,
    UserId UserId,
    ListingId ListingId) : IDomainEvent;
