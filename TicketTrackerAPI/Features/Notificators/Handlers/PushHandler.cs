using MediatR;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Services;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class PushHandler(
    IUserService _userService,
    NotificationProcessor<PushContent> _notificationProcessor,
    ILogger<PushHandler> _logger) : INotificationHandler<TicketCreatedNotification>
{
    public async Task Handle(TicketCreatedNotification request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending push notification for ticket creation.");

        var deviceToken = _userService.GetUserDeviceToken();

        var content = new PushContent
        {
            DeviceToken = deviceToken,
            Title = $"A ticket has been created: {request.Ticket.Title}",
            Body = request.Ticket.Description
        };

        await _notificationProcessor.ProcessNotification(content, request.Ticket, Channel.Push);
    }
}
