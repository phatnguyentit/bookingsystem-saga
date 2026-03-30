using BookingSystem.ReviewService.Api.Endpoints;
using BookingSystem.ReviewService.Infrastructure.Persistence;
using BookingSystem.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ReviewDbContext>("reviewdb");

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapReviewEndpoints();

app.Run();
