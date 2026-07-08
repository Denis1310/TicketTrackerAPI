namespace TicketTrackerAPI.Services;

public interface IUserService
{
    string GetUserEmail();
    string GetUserPhoneNumber();
    string GetUserDeviceToken();
}
