using MediatR;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Features.Notificators;

public class TicketCreatedNotification : INotification
{
    public Guid TicketId { get; }
    public Status Status { get; }
    public int Attemps { get; }

    public TicketCreatedNotification(Guid ticketId, Status status, int attemps)
    {
        TicketId = ticketId;
        Status = status;
        Attemps = attemps;
    }
}
