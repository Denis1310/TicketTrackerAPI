using TicketTrackerAPI.Models;

namespace TicketTrackerAPI.Services;

// Implementation for demonstration purposes only. In a real-world scenario,
// this service would integrate with a push notification provider.
public class PushService : INotificationService<PushContent>
{
    public async Task Send(PushContent notification)
    {
        await Task.Delay(200);
    }
}
