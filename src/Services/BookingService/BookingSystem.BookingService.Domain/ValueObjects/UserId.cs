namespace BookingSystem.BookingService.Domain.ValueObjects;

public record UserId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
