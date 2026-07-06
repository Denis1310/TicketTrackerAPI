using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Commands;

public class CreateTicket : IRequest
{
    public Ticket Ticket { get; set; }

    public CreateTicket(Ticket ticket)
    {
        Ticket = ticket;
    }
}
