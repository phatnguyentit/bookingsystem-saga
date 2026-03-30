using BookingSystem.UserService.Api.Endpoints;
using BookingSystem.ServiceDefaults;
using BookingSystem.UserService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<UserDbContext>("userdb");
builder.AddRedisDistributedCache("redis");

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapUserEndpoints();

app.Run();
