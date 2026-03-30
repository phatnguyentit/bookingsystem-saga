using BookingSystem.BookingService.Application.Interfaces;
using System.Net.Http.Json;

namespace BookingSystem.BookingService.Infrastructure.HttpClients;

public class UserServiceClient(HttpClient http) : IUserServiceClient
{
    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync($"/api/users/{userId}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
