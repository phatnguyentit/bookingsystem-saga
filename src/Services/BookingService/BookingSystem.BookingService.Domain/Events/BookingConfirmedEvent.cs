using BookingSystem.BookingService.Domain.Common;
using BookingSystem.BookingService.Domain.ValueObjects;

namespace BookingSystem.BookingService.Domain.Events;

public record BookingConfirmedEvent(BookingId BookingId) : IDomainEvent
{
    public static BookingConfirmedEvent Create(BookingId bookingId) => new(bookingId);
}