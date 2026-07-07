using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class SmsHandler(
    NotificationInMemoryRepository _repo,
    ILogger<SmsHandler> _logger) : INotificationHandler<TicketCreatedNotification>
{
    public Task Handle(TicketCreatedNotification request, CancellationToken cancellationToken)
    {
        _repo.AddNotification(new Notification
        {
            TicketId = request.TicketId,
            Channel = Channel.Sms,
            Status = request.Status,
            Attemps = request.Attemps
        });

        _logger.LogInformation("Sending sms notification for ticket creation.");
        return Task.CompletedTask;
    }
}
