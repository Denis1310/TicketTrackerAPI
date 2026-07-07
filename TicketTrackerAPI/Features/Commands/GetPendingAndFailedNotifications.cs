using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Commands;

public class GetPendingAndFailedNotifications : IRequest<List<Notification>>
{
    public Guid TicketId { get; }

    public GetPendingAndFailedNotifications(Guid ticketId)
    {
        TicketId = ticketId;
    }
}
