using MediatR;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Repositories;

namespace TicketTrackerAPI.Features.Handlers;

public class GetTicketByIdHandler(
    ITicketInMemoryRepository _repo) :
    IRequestHandler<GetTicketById, Ticket?>
{
    public Task<Ticket?> Handle(GetTicketById request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            _repo.GetTicketById(request.TicketId));
    }
}
