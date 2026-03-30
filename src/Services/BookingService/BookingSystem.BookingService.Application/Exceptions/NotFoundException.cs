namespace BookingSystem.BookingService.Application.Exceptions;

public class NotFoundException(string message) : Exception(message);
