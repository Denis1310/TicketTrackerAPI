using MediatR;
using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Features.Notificators;

public class TicketCreatedNotification : INotification
{
    public Notification Notification { get; }

    public TicketCreatedNotification(Notification notification)
    {
        Notification = notification;
    }
}
