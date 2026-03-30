namespace BookingSystem.BookingService.Domain.ValueObjects;

public record BookingId(Guid Value)
{
    public static BookingId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
