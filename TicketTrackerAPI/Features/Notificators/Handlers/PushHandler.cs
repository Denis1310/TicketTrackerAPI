using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class PushHandler(
    NotificationInMemoryRepository _repo,
    ILogger<PushHandler> _logger) : INotificationHandler<TicketCreatedNotification>
{
    public Task Handle(TicketCreatedNotification request, CancellationToken cancellationToken)
    {
        _repo.AddNotification(new Notification
        {
            TicketId = request.TicketId,
            Channel = Channel.Push,
            Status = request.Status,
            Attemps = request.Attemps
        });

        _logger.LogInformation("Sending push notification for ticket creation.");
        return Task.CompletedTask;
    }
}
