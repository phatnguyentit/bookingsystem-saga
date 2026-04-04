# Architecture & Design Patterns

Analysis of the `bookingsystem-saga` microservice architecture.

---

## Core Architectural Patterns

| Pattern | Location | Notes |
|---|---|---|
| **API Gateway** | `src/Gateway/BookingSystem.ApiGateway/` | YARP reverse proxy, JWT auth, rate limiting (100 req/min) |
| **Database per Microservice** | `src/Orchestration/BookingSystem.AppHost/Program.cs` | Each service has its own PostgreSQL DB (`userdb`, `catalogdb`, `bookingdb`, etc.) |
| **Event-Driven / Choreography** | `src/Shared/BookingSystem.Shared.Messaging/` | Kafka-based async communication; services react to events independently |
| **Service Discovery** | `src/ServiceDefaults/BookingSystem.ServiceDefaults/Extensions.cs` | .NET Aspire DNS-based service discovery |

---

## Domain-Driven Design (DDD) — `BookingService`

| Pattern | Location |
|---|---|
| **Aggregate Root** | `BookingService.Domain/Aggregates/Booking.cs` — state machine: Pending → Confirmed/Cancelled → Completed |
| **Value Objects** | `Domain/ValueObjects/` — `Money`, `DateRange`, `BookingId`, `UserId` (strongly-typed IDs) |
| **Domain Events** | `Domain/Events/` — `BookingCreatedEvent`, `BookingConfirmedEvent`, `BookingCancelledEvent` |
| **Repository Pattern** | `IBookingRepository` (Domain) → `BookingRepository` (Infrastructure/EF Core) |

---

## Application Layer Patterns

| Pattern | Location |
|---|---|
| **CQRS** | Commands (`CreateBookingCommand`, `CancelBookingCommand`) and Queries (`GetBookingQuery`) separated via MediatR — applied across all services |
| **Mediator** | MediatR `ISender`/`IPublisher` used in all services for request routing and domain event dispatching |
| **Unit of Work** | `BookingService.Infrastructure/Messaging/UnitOfWork.cs` — saves to DB, then dispatches domain events via MediatR |

---

## Infrastructure Patterns

| Pattern | Location |
|---|---|
| **Event Publishing** | `src/Shared/BookingSystem.Shared.Messaging/KafkaEventPublisher.cs` — Confluent.Kafka, JSON serialized, per-message Guid key |
| **Background Consumer** | `NotificationService/Api/Consumers/KafkaConsumers.cs` — `BackgroundService`-based Kafka consumers, manual commit |
| **Distributed Cache** | Redis on all services via `AddRedisDistributedCache()` |
| **Full-Text Search** | `SearchService/Infrastructure/Search/ElasticsearchService.cs` — Elasticsearch with pagination and date/price filtering |
| **Circuit Breaker / Resilience** | Via `StandardResilienceHandler` in ServiceDefaults (retries, timeouts, circuit breaker) |
| **Observability** | OpenTelemetry (traces + metrics + logs) across all services |

---

## Event Flow (Booking Workflow Example)

```
Client
  ↓ POST /api/bookings
APIGateway (YARP) [Rate limit, Auth]
  ↓ routes to booking-service
BookingService.Api/Endpoints/BookingEndpoints
  ↓ MediatR ISender.Send()
CreateBookingCommand → CreateBookingHandler
  ↓
  1. Call CatalogServiceClient.GetListingAsync() [HTTP to catalog-service]
  2. Validate: listing available, no overlaps
  3. Create Booking aggregate
  4. booking.AddDomainEvent(BookingCreatedEvent)
  5. bookingRepo.AddAsync(booking)
  6. unitOfWork.CommitAsync()
     ↓
     - DbContext.SaveChangesAsync() [Persist to DB]
     - Extract DomainEvents from ChangeTracker
     - MediatR Publish(BookingCreatedEvent)
       ↓
       PublishBookingCreatedHandler
         ↓
         IEventPublisher.PublishAsync("booking.created", BookingCreatedIntegrationEvent)
           ↓
           KafkaEventPublisher → Kafka topic "booking.created"

Parallel: Event Subscribers
  ↓
NotificationService.BookingCreatedKafkaConsumer (BackgroundService)
  ↓ Consume from "booking.created"
  ↓ INotificationSender.SendEmailAsync(userId, message)
  ↓ Commit offset

Optional: PaymentService listens for booking.created
  → Initiates payment process
  → Publishes payment.succeeded or payment.failed
    ↓
    NotificationService receives and sends email
```

---

## Inter-Service Communication

**Synchronous (HTTP)**
- BookingService → CatalogService (verify listing exists/available)
- BookingService → UserService (validation)
- All via `HttpClient` with Aspire service discovery

**Asynchronous (Kafka)**
- `booking.created` — BookingService → NotificationService
- `booking.cancelled` — BookingService → (subscribers)
- `payment.succeeded` — PaymentService → NotificationService
- `payment.failed` — PaymentService → NotificationService

---

## Notable Design Decisions & Gaps

### What's there
- **Choreography-based Saga** — no central coordinator; services react to Kafka events independently (this is the "Saga" in the repo name)
- **Domain → Integration event translation** — MediatR handlers convert domain events to integration events before publishing to Kafka
- **Unit of Work** publishes domain events after DB commit, keeping persistence and messaging in sync within a request
- **Clean Architecture** per service: Domain → Application → Infrastructure → API

### What's NOT implemented
| Gap | Risk |
|---|---|
| **Outbox Pattern** | If Kafka publish fails after DB commit, events are silently lost |
| **Orchestration-based Saga** | No central saga coordinator or state machine for long-running workflows |
| **Event Sourcing** | Events are dispatched in-memory only; not persisted as the source of truth |
