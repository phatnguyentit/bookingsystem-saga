using BookingSystem.BookingService.Domain.Common;
using System.Text.Json;

namespace BookingSystem.BookingService.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string EventType { get; private set; } = default!;   // assembly-qualified type name
    public string Payload { get; private set; } = default!;     // JSON-serialized domain event
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? Error { get; private set; }

    private OutboxMessage() { }

    public static OutboxMessage From(IDomainEvent domainEvent) => new()
    {
        Id = Guid.NewGuid(),
        EventType = domainEvent.GetType().AssemblyQualifiedName!,
        Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
        CreatedAt = DateTime.UtcNow
    };

    public void MarkProcessed() => ProcessedAt = DateTime.UtcNow;

    public void MarkFailed(string error) => Error = error;
}