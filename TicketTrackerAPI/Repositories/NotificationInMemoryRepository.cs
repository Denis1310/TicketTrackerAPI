using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Repositories;

public class NotificationInMemoryRepository : INotificationInMemoryRepository
{
    List<Notification> _notifications = new List<Notification>();

    public void AddRange(List<Notification> notifications)
    {
        _notifications.AddRange(notifications);
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
