using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class CreateTicketHandler(
    TicketInMemoryRepository _repo) : IRequestHandler<CreateTicket, Ticket>
{
    public Task<Ticket> Handle(CreateTicket request, CancellationToken cancellationToken)
    {
        _repo.AddTicket(request.Ticket);
        return Task.FromResult(request.Ticket);
    }
}
