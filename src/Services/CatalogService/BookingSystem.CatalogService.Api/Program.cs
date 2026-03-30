using BookingSystem.CatalogService.Api.Endpoints;
using BookingSystem.CatalogService.Infrastructure.Persistence;
using BookingSystem.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<CatalogDbContext>("catalogdb");
builder.AddRedisDistributedCache("redis");

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapCatalogEndpoints();

app.Run();
