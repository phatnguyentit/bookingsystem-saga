using BookingSystem.BookingService.Application.Exceptions;
using BookingSystem.BookingService.Application.Interfaces;
using BookingSystem.BookingService.Domain.Repositories;
using BookingSystem.BookingService.Domain.ValueObjects;
using MediatR;

namespace BookingSystem.BookingService.Application.Commands.ConfirmBooking;

public class ConfirmBookingHandler(
    IBookingRepository bookingRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<ConfirmBookingCommand>
{
    public async Task Handle(ConfirmBookingCommand cmd, CancellationToken cancellationToken)
    {
        var booking = await bookingRepo.GetByIdAsync(new BookingId(cmd.BookingId), cancellationToken)
            ?? throw new NotFoundException($"Booking {cmd.BookingId} not found.");

        booking.Confirm();
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
