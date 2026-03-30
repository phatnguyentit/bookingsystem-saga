using BookingSystem.NotificationService.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace BookingSystem.NotificationService.Infrastructure.Services;

public interface INotificationSender
{
    Task SendEmailAsync(Guid recipientId, string message, CancellationToken cancellationToken = default);
}

public class EmailNotificationSender(
    NotifDbContext db,
    ILogger<EmailNotificationSender> logger) : INotificationSender
{
    public async Task SendEmailAsync(Guid recipientId, string message, CancellationToken cancellationToken = default)
    {
        // In production: call SMTP/SendGrid/SES here
        logger.LogInformation(
            "Sending email to user {UserId}: {Message}", recipientId, message);

        db.NotificationLogs.Add(new NotificationLog
        {
            Id = Guid.NewGuid(),
            RecipientId = recipientId,
            Message = message,
            Channel = "Email",
            IsDelivered = true,
            SentAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
