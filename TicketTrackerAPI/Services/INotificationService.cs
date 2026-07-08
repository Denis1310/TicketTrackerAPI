using TicketTrackerAPI.Models;

namespace TicketTrackerAPI.Services;

public interface INotificationService<TNotification>
    where TNotification : INotificationContent, new()
{
    Task Send(TNotification notification);
}
