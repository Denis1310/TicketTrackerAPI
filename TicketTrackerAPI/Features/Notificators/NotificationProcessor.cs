using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Repositories;
using TicketTrackerAPI.Services;

namespace TicketTrackerAPI.Features.Notificators;

public class NotificationProcessor<TNotification>(
    INotificationService<TNotification> _notificationService,
    NotificationInMemoryRepository _repo,
    ILogger<NotificationProcessor<TNotification>> _logger)
    where TNotification : INotificationContent, new()
{
    public async Task ProcessNotification(TNotification notificationContent,
        Ticket ticket,
        Channel channel)
    {
        var notification = new Notification();
        var existingNotification = _repo.GetByTicketIdAndChannel(ticket.Id, channel);

        if (existingNotification != null)
        {
            if (existingNotification.Status == Status.Sent ||
                existingNotification.Attemps >= 3)
            {
                _logger.LogInformation($"The notification for ticket {ticket.Id} has already been sent or reached maximum attempts");
                return;
            }

            notification = existingNotification;
        }
        else
        {
            notification = new Notification
            {
                TicketId = ticket.Id,
                Channel = channel,
                Status = Status.Pending
            };

            _repo.AddNotification(notification);
        }

        try
        {
            await _notificationService.Send(notificationContent);

            notification.Status = Status.Sent;
            _repo.UpdateNotification(notification);

            _logger.LogInformation("The notification sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while sending the notification: {ex.Message}.");

            notification.Status = Status.Failed;
            notification.LastError = ex.Message;
            notification.Attemps++;
            _repo.UpdateNotification(notification);
        }
    }
}
