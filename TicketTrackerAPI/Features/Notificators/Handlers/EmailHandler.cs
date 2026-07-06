using MediatR;

namespace TicketTrackerAPI.Features.Notificators.Handlers;

public class EmailHandler : INotificationHandler<TicketCreatedNotification>
{
    public Task Handle(TicketCreatedNotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
