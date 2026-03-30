namespace BookingSystem.BookingService.Domain.Common;

public abstract class AggregateRoot<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected set; } = default!;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

// Non-generic base for EF change tracker enumeration
public abstract class AggregateRoot : AggregateRoot<Guid>;
