using BookingSystem.BookingService.Domain.ValueObjects;

namespace BookingSystem.BookingService.Application.Exceptions;

public class ListingNotAvailableException(Guid listingId, DateRange period)
    : Exception($"Listing {listingId} is not available from {period.CheckIn} to {period.CheckOut}.");
