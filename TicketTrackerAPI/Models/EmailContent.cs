namespace TicketTrackerAPI.Models;

public class EmailContent : INotificationContent
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string TextBody { get; set; }
}
