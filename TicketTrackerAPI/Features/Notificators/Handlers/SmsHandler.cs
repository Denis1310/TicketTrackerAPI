using MediatR;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Services;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class SmsHandler(
    IUserService _userService,
    NotificationProcessor<SmsContent> _notificationProcessor,
    ILogger<SmsHandler> _logger) : INotificationHandler<TicketCreatedNotification>
{
    public async Task Handle(TicketCreatedNotification request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending sms notification for ticket creation...");

        var phoneNumber = _userService.GetUserPhoneNumber();
        var content = new SmsContent
        {
            PhoneNumber = phoneNumber,
            Message = $"A new ticket has been created: {request.Ticket.Title}"
        };

        await _notificationProcessor.ProcessNotification(content, request.Ticket, Channel.Sms);
    }
}
