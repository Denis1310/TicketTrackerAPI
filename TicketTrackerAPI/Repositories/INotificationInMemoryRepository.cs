using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Repositories;

public interface INotificationInMemoryRepository
{
    void AddRange(List<Notification> notifications);
    List<Notification> GetAllNotifications(Guid ticketId);
    void UpdateNotification(Notification notification);
    Notification? GetByTicketIdAndChannel(Guid ticketId, Channel channel);
}
