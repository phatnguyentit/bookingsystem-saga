using BookingSystem.SearchService.Api.Endpoints;
using BookingSystem.SearchService.Infrastructure.Search;
using BookingSystem.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddElasticsearchClient("elasticsearch");
builder.AddRedisDistributedCache("redis");

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSingleton<ISearchService, ElasticsearchService>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapSearchEndpoints();

app.Run();
