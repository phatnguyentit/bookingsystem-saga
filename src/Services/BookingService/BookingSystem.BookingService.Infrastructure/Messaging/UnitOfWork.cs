using BookingSystem.BookingService.Application.Interfaces;
using BookingSystem.BookingService.Domain;
using BookingSystem.BookingService.Domain.Common;
using BookingSystem.BookingService.Infrastructure.Persistence;
using MediatR;

namespace BookingSystem.BookingService.Infrastructure.Messaging;

public class UnitOfWork(BookingDbContext db, IPublisher publisher) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var events = db.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        await db.SaveChangesAsync(cancellationToken);

        foreach (var evt in events)
            await publisher.Publish(evt, cancellationToken);

        // Clear events after publishing to prevent re-dispatch
        foreach (var entry in db.ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearDomainEvents();
    }
}
