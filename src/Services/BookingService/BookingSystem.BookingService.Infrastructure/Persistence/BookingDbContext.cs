using BookingSystem.BookingService.Domain;
using BookingSystem.BookingService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingSystem.BookingService.Infrastructure.Persistence;

public class BookingDbContext(DbContextOptions<BookingDbContext> options)
    : DbContext(options)
{
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public async Task MigrateWithRetryAsync(ILogger<BookingDbContext> logger, int attempts, TimeSpan delay)
    {
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                await this.Database.MigrateAsync();
                break;
            }
            catch (Exception ex) when (attempt < attempts)
            {
                logger.LogWarning(ex, "Migration attempt {Attempt} failed. Retrying in {Delay}s...", attempt, delay.TotalSeconds);
                await Task.Delay(delay);
                delay *= 2;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
    }
}
