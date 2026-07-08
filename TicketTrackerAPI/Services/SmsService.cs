using TicketTrackerAPI.Models;

namespace TicketTrackerAPI.Services;

// Implementation for demonstration purposes only.
// In real application, it would use an SMS service or library.
public class SmsService : INotificationService<SmsContent>
{
    public async Task Send(SmsContent notification)
    {
        await Task.Delay(200);
    }
}
