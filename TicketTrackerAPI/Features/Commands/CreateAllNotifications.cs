using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Features.Commands;

public class CreateAllNotifications : IRequest
{
    public Ticket Ticket { get; }
    public IEnumerable<Channel> Channels { get; }

    public CreateAllNotifications(Ticket ticket, IEnumerable<Channel> channels)
    {
        Ticket = ticket;
        Channels = channels;
    }
}
