using MediatR;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Services;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class EmailHandler(
    IUserService _userService,
    NotificationProcessor<EmailContent> _notificationProcessor,
    ILogger<EmailHandler> _logger) : INotificationHandler<TicketCreatedNotification>
{
    public async Task Handle(TicketCreatedNotification request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending email notification for ticket creation...");

        var userEmail = _userService.GetUserEmail();

        var description = !string.IsNullOrWhiteSpace(request.Ticket.Description) ?
                    request.Ticket.Description :
                    "missing";

        var content = new EmailContent
        {
            Subject = userEmail,
            TextBody = $"You have a new ticket: {request.Ticket.Title}!",
            Body = $@"""<h3>A new ticket has been created: {request.Ticket.Title}.</h3>
                        <p><b>Priority:</b> {request.Ticket.Priority}.</p>
                        <p><b>Description:</b> {description}.</p>"""
        };

        await _notificationProcessor.ProcessNotification(content, request.Ticket, Channel.Email);
    }
}
