var builder = DistributedApplication.CreateBuilder(args);

// --- Infrastructure ---
var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var redis    = builder.AddRedis("redis").WithRedisCommander();
var kafka    = builder.AddKafka("kafka").WithKafkaUI();
var elastic  = builder.AddElasticsearch("elasticsearch");

// --- Databases (one per service) ---
var userDb    = postgres.AddDatabase("userdb");
var catalogDb = postgres.AddDatabase("catalogdb");
var bookingDb = postgres.AddDatabase("bookingdb");
var paymentDb = postgres.AddDatabase("paymentdb");
var notifDb   = postgres.AddDatabase("notifdb");
var reviewDb  = postgres.AddDatabase("reviewdb");

// --- Core Services ---
var userSvc = builder.AddProject<Projects.BookingSystem_UserService_Api>("user-service")
    .WithReference(userDb)
    .WithReference(redis);

var catalogSvc = builder.AddProject<Projects.BookingSystem_CatalogService_Api>("catalog-service")
    .WithReference(catalogDb)
    .WithReference(redis);

var bookingSvc = builder.AddProject<Projects.BookingSystem_BookingService_Api>("booking-service")
    .WithReference(bookingDb)
    .WithReference(redis)
    .WithReference(kafka)
    .WithReference(userSvc)
    .WithReference(catalogSvc);

var paymentSvc = builder.AddProject<Projects.BookingSystem_PaymentService_Api>("payment-service")
    .WithReference(paymentDb)
    .WithReference(kafka);

var notifSvc = builder.AddProject<Projects.BookingSystem_NotificationService_Api>("notification-service")
    .WithReference(notifDb)
    .WithReference(kafka)
    .WithReference(redis);

var searchSvc = builder.AddProject<Projects.BookingSystem_SearchService_Api>("search-service")
    .WithReference(elastic)
    .WithReference(redis)
    .WithReference(kafka);

var reviewSvc = builder.AddProject<Projects.BookingSystem_ReviewService_Api>("review-service")
    .WithReference(reviewDb)
    .WithReference(kafka);

// --- API Gateway ---
builder.AddProject<Projects.BookingSystem_ApiGateway>("api-gateway")
    .WithReference(userSvc)
    .WithReference(catalogSvc)
    .WithReference(bookingSvc)
    .WithReference(paymentSvc)
    .WithReference(searchSvc)
    .WithReference(reviewSvc)
    .WithExternalHttpEndpoints();

builder.Build().Run();
