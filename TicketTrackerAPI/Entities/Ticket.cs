using System.ComponentModel.DataAnnotations;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
    public string Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
