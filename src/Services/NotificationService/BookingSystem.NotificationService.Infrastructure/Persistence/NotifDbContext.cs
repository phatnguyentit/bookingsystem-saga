using Microsoft.EntityFrameworkCore;

namespace BookingSystem.NotificationService.Infrastructure.Persistence;

public class NotificationLog
{
    public Guid Id { get; set; }
    public Guid RecipientId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Channel { get; set; } = "Email";
    public bool IsDelivered { get; set; }
    public DateTime SentAt { get; set; }
}

public class NotifDbContext(DbContextOptions<NotifDbContext> options) : DbContext(options)
{
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<NotificationLog>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Message).HasMaxLength(1000);
            e.Property(n => n.Channel).HasMaxLength(20);
            e.ToTable("notification_logs");
        });
    }
}
