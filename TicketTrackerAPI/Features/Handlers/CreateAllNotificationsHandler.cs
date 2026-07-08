using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class CreateAllNotificationsHandler(
    NotificationInMemoryRepository _repo,
    ILogger<CreateAllNotificationsHandler> _logger) : IRequestHandler<CreateAllNotifications>
{
    public Task Handle(CreateAllNotifications request, CancellationToken cancellationToken)
    {
        var notifications = request.Channels.Select(channel => new Notification
        {
            TicketId = request.Ticket.Id,
            Channel = channel,
            Status = Status.Pending
        });

        _repo.AddRange(notifications.ToList());

        _logger.LogInformation(
            "Created {Count} notifications for ticket {TicketId}",
            notifications.Count(), request.Ticket.Id);

        return Task.CompletedTask;
    }
}