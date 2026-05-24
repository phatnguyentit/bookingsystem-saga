using BookingSystem.BookingService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.BookingService.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.EventType).IsRequired().HasMaxLength(500);
        builder.Property(m => m.Payload).IsRequired();
        builder.Property(m => m.Error).HasMaxLength(2000);

        // index for the background processor query: unprocessed messages ordered by creation
        builder.HasIndex(m => new { m.ProcessedAt, m.CreatedAt });

        builder.ToTable("outbox_messages");
    }
}