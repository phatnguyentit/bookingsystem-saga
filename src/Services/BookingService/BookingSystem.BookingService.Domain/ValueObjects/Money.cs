using BookingSystem.BookingService.Domain.Exceptions;

namespace BookingSystem.BookingService.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new BookingDomainException("Cannot add different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}
