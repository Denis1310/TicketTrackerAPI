using MediatR;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class CreateTicketHandler(
    TicketRepository _repo) : IRequestHandler<CreateTicket>
{
    public Task Handle(CreateTicket request, CancellationToken cancellationToken)
    {
        _repo.AddTicket(request.Ticket);
        return Task.CompletedTask;
    }
}
