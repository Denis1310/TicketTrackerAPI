using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Commands;

public class GetTicketById : IRequest<Ticket>
{
    public Guid TicketId { get; }

    public GetTicketById(Guid ticketId)
    {
        TicketId = ticketId;
    }
}
