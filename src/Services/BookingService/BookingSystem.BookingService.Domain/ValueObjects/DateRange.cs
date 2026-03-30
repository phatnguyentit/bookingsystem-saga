using BookingSystem.BookingService.Domain.Exceptions;

namespace BookingSystem.BookingService.Domain.ValueObjects;

public record DateRange
{
    public DateOnly CheckIn { get; }
    public DateOnly CheckOut { get; }

    public DateRange(DateOnly checkIn, DateOnly checkOut)
    {
        if (checkOut <= checkIn)
            throw new BookingDomainException("CheckOut must be after CheckIn.");
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;
}
