using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Features.Notificators;

namespace TicketTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController(
    IMediator _mediator) : ControllerBase
{
    [HttpPost("tickets")]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
        try
        {
            await _mediator.Send(new CreateTicket(ticket));
        }
        catch
        {
            return StatusCode(500, "An error occurred while creating the ticket.");
        }

        var notification = new Notification()
        {
            TicketId = ticket.Id,
            Status = Status.Pending,
            Attemps = 1,
        };

        await _mediator.Publish(new TicketCreatedNotification(notification));

        return Ok();
    }

    [HttpPost("tickets/{id}/notify")]
    public IActionResult NotifyTicket(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("tickets/{id}")]
    public IActionResult GetTicketWithNotificationsById(Guid id)
    {
        throw new NotImplementedException();
    }
}
