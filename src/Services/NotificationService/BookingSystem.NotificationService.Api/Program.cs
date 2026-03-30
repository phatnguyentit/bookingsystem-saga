using BookingSystem.NotificationService.Api.Consumers;
using BookingSystem.NotificationService.Infrastructure.Persistence;
using BookingSystem.NotificationService.Infrastructure.Services;
using BookingSystem.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<NotifDbContext>("notifdb");
builder.AddRedisDistributedCache("redis");

builder.Services.AddScoped<INotificationSender, EmailNotificationSender>();

// Register Kafka consumers as hosted services
builder.Services.AddHostedService<BookingCreatedKafkaConsumer>();
builder.Services.AddHostedService<PaymentSucceededKafkaConsumer>();
builder.Services.AddHostedService<PaymentFailedKafkaConsumer>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
