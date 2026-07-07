using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class GetTicketWithNotificationsHandler(
    TicketInMemoryRepository _ticketRepo,
    NotificationInMemoryRepository _notificationRepo) : IRequestHandler<GetTicketWithNotifications, Ticket?>
{
    public Task<Ticket?> Handle(GetTicketWithNotifications request, CancellationToken cancellationToken)
    {
        var ticket = _ticketRepo.GetTicketById(request.TicketId);

        if (ticket is null)
        {
            return Task.FromResult(ticket);
        }

        var notifications = _notificationRepo.GetAllNotifications(request.TicketId);
        ticket.Notifications = notifications;

        return Task.FromResult<Ticket?>(ticket);
    }
}
