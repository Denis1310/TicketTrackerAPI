using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Repositories;

public class NotificationInMemoryRepository
{
    List<Notification> _notifications = new List<Notification>();

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }

    public List<Notification> GetAllPendingAndFailedNotifications(Guid ticketId)
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

    public void UpdateNotification(Notification notification)
    {
        var existingNotification = _notifications.FirstOrDefault(n => n.Id == notification.Id);

        if (existingNotification != null)
        {
            existingNotification.Status = notification.Status;
            existingNotification.Attemps = notification.Attemps;
            existingNotification.LastError = notification.LastError;
        }
    }

    public Notification? GetByTicketIdAndChannel(Guid ticketId, Channel channel)
    {
        return _notifications
            .FirstOrDefault(n => n.TicketId == ticketId && n.Channel == channel);
    }
}
