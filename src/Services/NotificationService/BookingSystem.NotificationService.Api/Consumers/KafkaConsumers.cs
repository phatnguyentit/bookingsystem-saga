using BookingSystem.NotificationService.Infrastructure.Services;
using BookingSystem.Shared.Contracts.Events;
using Confluent.Kafka;
using System.Text.Json;

namespace BookingSystem.NotificationService.Api.Consumers;

public abstract class KafkaConsumerBase<T>(
    string topic,
    IConfiguration configuration,
    ILogger logger) : BackgroundService where T : class
{
    protected abstract Task ProcessAsync(T message, CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = $"notification-service-{topic}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                if (result?.Message?.Value is null) continue;

                try
                {
                    var message = JsonSerializer.Deserialize<T>(result.Message.Value);
                    if (message is not null)
                        await ProcessAsync(message, stoppingToken);
                    consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message from topic {Topic}", topic);
                }
            }
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        finally
        {
            consumer.Close();
        }
    }
}

public class BookingCreatedKafkaConsumer(
    IConfiguration configuration,
    ILogger<BookingCreatedKafkaConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : KafkaConsumerBase<BookingCreatedIntegrationEvent>(
        "booking.created", configuration, logger)
{
    protected override async Task ProcessAsync(BookingCreatedIntegrationEvent message, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<INotificationSender>();
        await sender.SendEmailAsync(
            message.UserId,
            $"Your booking {message.BookingId} has been created!", cancellationToken);
    }
}

public class PaymentSucceededKafkaConsumer(
    IConfiguration configuration,
    ILogger<PaymentSucceededKafkaConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : KafkaConsumerBase<BookingSystem.Shared.Contracts.Events.PaymentSucceededIntegrationEvent>(
        "payment.succeeded", configuration, logger)
{
    protected override async Task ProcessAsync(
        BookingSystem.Shared.Contracts.Events.PaymentSucceededIntegrationEvent message,
        CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<INotificationSender>();
        await sender.SendEmailAsync(
            message.UserId,
            $"Payment of {message.Amount} {message.Currency} for booking {message.BookingId} succeeded.", cancellationToken);
    }
}

public class PaymentFailedKafkaConsumer(
    IConfiguration configuration,
    ILogger<PaymentFailedKafkaConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : KafkaConsumerBase<BookingSystem.Shared.Contracts.Events.PaymentFailedIntegrationEvent>(
        "payment.failed", configuration, logger)
{
    protected override async Task ProcessAsync(
        BookingSystem.Shared.Contracts.Events.PaymentFailedIntegrationEvent message,
        CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<INotificationSender>();
        await sender.SendEmailAsync(
            message.UserId,
            $"Payment for booking {message.BookingId} failed: {message.Reason}.", cancellationToken);
    }
}
