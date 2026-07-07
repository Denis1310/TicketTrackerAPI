using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Commands;

public class GetTicketWithNotifications : IRequest<Ticket?>
{
    public Guid TicketId { get; }
    public GetTicketWithNotifications(Guid ticketId)
    {
        TicketId = ticketId;
    }
}
