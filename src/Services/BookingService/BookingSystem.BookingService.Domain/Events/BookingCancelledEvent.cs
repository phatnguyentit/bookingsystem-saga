using BookingSystem.BookingService.Domain.Common;
using BookingSystem.BookingService.Domain.ValueObjects;

namespace BookingSystem.BookingService.Domain.Events;

public record BookingCancelledEvent(BookingId BookingId, string Reason) : IDomainEvent
{
    public static BookingCancelledEvent Create(BookingId bookingId, string reason) => new(bookingId, reason);
}
