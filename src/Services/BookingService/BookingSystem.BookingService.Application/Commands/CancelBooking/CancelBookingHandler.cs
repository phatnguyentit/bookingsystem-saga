using BookingSystem.BookingService.Application.Exceptions;
using BookingSystem.BookingService.Application.Interfaces.UoW;
using BookingSystem.BookingService.Domain.Repositories;
using BookingSystem.BookingService.Domain.ValueObjects;
using MediatR;

namespace BookingSystem.BookingService.Application.Commands.CancelBooking;

public class CancelBookingHandler(
    IBookingRepository bookingRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand cmd, CancellationToken cancellationToken)
    {
        var booking = await bookingRepo.GetByIdAsync(new BookingId(cmd.BookingId), cancellationToken)
            ?? throw new NotFoundException($"Booking {cmd.BookingId} not found.");

        booking.Cancel(cmd.Reason);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
