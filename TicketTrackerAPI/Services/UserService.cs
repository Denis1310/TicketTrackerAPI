namespace TicketTrackerAPI.Services;

// Implementatin for demonstration purposes, in a real application this would be
// implemented to retrieve the current user's information from the authentication
// context or user session.
public class UserService : IUserService
{
    public string GetUserEmail() => "user@example.com";
    public string GetUserPhoneNumber() => "+1234567890";
    public string GetUserDeviceToken() => "device_token_123";
}
