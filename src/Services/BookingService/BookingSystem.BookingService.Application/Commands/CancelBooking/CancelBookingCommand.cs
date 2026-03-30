using MediatR;

namespace BookingSystem.BookingService.Application.Commands.CancelBooking;

public record CancelBookingCommand(Guid BookingId, string Reason) : IRequest;
