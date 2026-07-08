using TicketTrackerAPI.Models;

namespace TicketTrackerAPI.Services;

// Implementation for demonstration purposes only. In a real application,
// it would use an email sending library or service.
public class EmailService : INotificationService<EmailContent>
{
    public async Task Send(EmailContent notification)
    {
        await Task.Delay(200);
    }
}
