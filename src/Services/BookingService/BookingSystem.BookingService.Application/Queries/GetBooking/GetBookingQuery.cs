using BookingSystem.BookingService.Application.DTOs;
using MediatR;

namespace BookingSystem.BookingService.Application.Queries.GetBooking;

public record GetBookingQuery(Guid BookingId) : IRequest<BookingDto>;
