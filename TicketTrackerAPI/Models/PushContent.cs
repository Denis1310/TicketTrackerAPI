namespace TicketTrackerAPI.Models;

public class PushContent : INotificationContent
{
    public string DeviceToken { get; set; }
    public string? Body { get; set; }
    public string Title { get; set; }
}
