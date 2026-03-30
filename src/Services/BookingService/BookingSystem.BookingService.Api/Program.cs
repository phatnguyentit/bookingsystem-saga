using BookingSystem.BookingService.Api.Endpoints;
using BookingSystem.BookingService.Application.Commands.CreateBooking;
using BookingSystem.BookingService.Application.Interfaces;
using BookingSystem.BookingService.Domain.Repositories;
using BookingSystem.BookingService.Infrastructure.HttpClients;
using BookingSystem.BookingService.Infrastructure.Messaging;
using BookingSystem.BookingService.Infrastructure.Persistence;
using BookingSystem.BookingService.Infrastructure.Repositories;
using BookingSystem.ServiceDefaults;
using BookingSystem.Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<BookingDbContext>("bookingdb");
builder.AddRedisDistributedCache("redis");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpClient<ICatalogServiceClient, CatalogServiceClient>(
    c => c.BaseAddress = new Uri("http://catalog-service"));
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(
    c => c.BaseAddress = new Uri("http://user-service"));

builder.Services.AddSingleton<IEventPublisher>(sp =>
{
    var bootstrapServers = builder.Configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
    var logger = sp.GetRequiredService<ILogger<KafkaEventPublisher>>();
    return new KafkaEventPublisher(bootstrapServers, logger);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapBookingEndpoints();

app.Run();
