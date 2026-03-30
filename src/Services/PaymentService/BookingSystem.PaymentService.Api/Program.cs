using BookingSystem.PaymentService.Api.Endpoints;
using BookingSystem.PaymentService.Infrastructure.Persistence;
using BookingSystem.ServiceDefaults;
using BookingSystem.Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<PaymentDbContext>("paymentdb");

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSingleton<IEventPublisher>(sp =>
{
    var bootstrapServers = builder.Configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
    var logger = sp.GetRequiredService<ILogger<KafkaEventPublisher>>();
    return new KafkaEventPublisher(bootstrapServers, logger);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapPaymentEndpoints();

app.Run();
