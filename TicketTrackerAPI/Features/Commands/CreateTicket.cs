using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Commands;

public class CreateTicket : IRequest<Ticket>
{
    public Ticket Ticket { get; }

    public CreateTicket(Ticket ticket)
    {
        Ticket = ticket;
    }
}
