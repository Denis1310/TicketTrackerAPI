using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class GetPendingAndFailedNotificationsHandler(
    NotificationInMemoryRepository _repo) :
    IRequestHandler<GetPendingAndFailedNotifications, List<Notification>>
{
    public Task<List<Notification>> Handle(GetPendingAndFailedNotifications request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            _repo.GetAllPendingAndFailedNotifications(request.TicketId));
    }
}
