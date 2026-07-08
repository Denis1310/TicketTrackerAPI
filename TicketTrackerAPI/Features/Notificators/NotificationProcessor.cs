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
        var notification = _repo.GetByTicketIdAndChannel(ticket.Id, channel);

        if (notification is null ||
            notification.Status == Status.Sent ||
            notification.Attemps >= 3)
        {
            _logger.LogInformation(
                "Notification for ticket {TicketId} is missing, already sent, or reached maximum attempts.",
                ticket.Id);

            return;
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
