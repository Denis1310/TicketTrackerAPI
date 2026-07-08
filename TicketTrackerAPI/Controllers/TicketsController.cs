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

            await _mediator.Send(new CreateAllNotifications(ticket,
                new List<Channel> { Channel.Email, Channel.Sms, Channel.Push }));

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
        var ticket = await _mediator.Send(new GetTicketById(id));

        if (ticket is null)
        {
            return NotFound();
        }

        await _mediator.Publish(new TicketCreatedNotification(ticket));

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketWithNotifications(Guid id)
    {
        var result = await _mediator.Send(new GetTicketWithNotifications(id));

        return result != null ?
            Ok(result) :
            NotFound();
    }
}
