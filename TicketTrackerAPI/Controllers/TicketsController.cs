using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Features.Commands;
using TicketTrackerAPI.Features.Notificators;
using TicketTrackerAPI.Models.DTOs;

namespace TicketTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController(
    IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
    {
        try
        {
            var ticket = await _mediator.Send(new CreateTicket(
                new Ticket
                {
                    Title = request.Title,
                    Description = request.Description,
                    Priority = request.Priority
                }));

            await _mediator.Publish(new TicketCreatedNotification(ticket.Id,
                Status.Pending,
                // TODO: Move to handler and implement retry logic.
                attemps: 1));

            return Created($"api/tickets/{ticket.Id}", ticket);
        }
        catch
        {
            return StatusCode(500, "An error occurred while creating the ticket.");
        }
    }

    [HttpPost("{id}/notify")]
    public async Task<IActionResult> NotifyTicket(Guid id)
    {
        var result = await _mediator.Send(new GetPendingAndFailedNotifications(id));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketWithNotifications(Guid id)
    {
        var result = await _mediator.Send(new GetTicketWithNotifications(id));

        return result is null ?
            NotFound() :
            Ok(result);
    }
}
