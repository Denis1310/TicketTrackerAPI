using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Repositories;

public class NotificationRepository(
    List<Notification> _notifications)
{
    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }

    public List<Notification> GetAllPendingOrFailedNotifications(Guid ticketId)
    {
        return _notifications
            .Where(n =>
                n.TicketId == ticketId && 
                (n.Status == Status.Pending ||
                n.Status == Status.Failed)).ToList();
    }

    public List<Notification> GetAllNotifications(Guid ticketId)
    {
        return _notifications
            .Where(n => n.TicketId == ticketId)
            .ToList();
    }
}
