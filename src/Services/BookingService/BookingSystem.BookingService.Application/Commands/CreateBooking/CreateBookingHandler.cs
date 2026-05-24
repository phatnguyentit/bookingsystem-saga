using BookingSystem.BookingService.Application.Exceptions;
using BookingSystem.BookingService.Application.Interfaces;
using BookingSystem.BookingService.Application.Interfaces.UoW;
using BookingSystem.BookingService.Domain;
using BookingSystem.BookingService.Domain.Repositories;
using BookingSystem.BookingService.Domain.ValueObjects;
using MediatR;

namespace BookingSystem.BookingService.Application.Commands.CreateBooking;

public class CreateBookingHandler(
    IBookingRepository bookingRepo,
    ICatalogServiceClient catalogClient,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateBookingCommand, BookingId>
{
    public async Task<BookingId> Handle(CreateBookingCommand cmd, CancellationToken cancellationToken)
    {
        var period = new DateRange(cmd.CheckIn, cmd.CheckOut);

        var listing = await catalogClient.GetListingAsync(cmd.ListingId, cancellationToken)
            ?? throw new NotFoundException($"Listing {cmd.ListingId} not found.");

        if (!listing.IsAvailable)
            throw new ListingNotAvailableException(cmd.ListingId, period);

        if (await bookingRepo.HasOverlapAsync(new ListingId(cmd.ListingId), period, cancellationToken))
            throw new BookingOverlapException();

        var (amount, currency) = (listing.PricePerNight * period.Nights, listing.Currency);
        var totalPrice = new Money(amount, currency);

        var booking = Booking.Create(
            new UserId(cmd.UserId),
            new ListingId(cmd.ListingId),
            period,
            totalPrice);

        await bookingRepo.AddAsync(booking, cancellationToken);
        // Other repos (e.g. for availability) would be updated here as well
        await unitOfWork.CommitAsync(cancellationToken);

        return booking.Id;
    }
}
