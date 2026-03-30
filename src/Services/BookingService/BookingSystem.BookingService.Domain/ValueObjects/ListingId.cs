namespace BookingSystem.BookingService.Domain.ValueObjects;

public record ListingId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
