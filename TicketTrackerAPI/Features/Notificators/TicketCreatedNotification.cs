using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Notificators;

public class TicketCreatedNotification : INotification
{
    public Ticket Ticket { get; }

    public TicketCreatedNotification(Ticket ticket)
    {
        Ticket = ticket;
    }
}
