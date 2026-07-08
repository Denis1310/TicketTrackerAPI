namespace TicketTrackerAPI.Models;

public class SmsContent : INotificationContent
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}
