using BookingSystem.BookingService.Domain;
using BookingSystem.BookingService.Domain.Repositories;
using BookingSystem.BookingService.Domain.ValueObjects;
using BookingSystem.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.BookingService.Infrastructure.Repositories;

public class BookingRepository(BookingDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default)
        => db.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        => await db.Bookings.AddAsync(booking, cancellationToken);

    public Task<bool> HasOverlapAsync(ListingId listingId, DateRange period, CancellationToken cancellationToken = default)
        => db.Bookings.AnyAsync(b =>
            b.ListingId == listingId &&
            b.Status != BookingStatus.Cancelled &&
            b.Period.CheckIn < period.CheckOut &&
            b.Period.CheckOut > period.CheckIn, cancellationToken);
}
