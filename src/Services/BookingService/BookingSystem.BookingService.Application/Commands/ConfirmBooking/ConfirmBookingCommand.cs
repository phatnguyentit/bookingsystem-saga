using MediatR;

namespace BookingSystem.BookingService.Application.Commands.ConfirmBooking;

public record ConfirmBookingCommand(Guid BookingId) : IRequest;
