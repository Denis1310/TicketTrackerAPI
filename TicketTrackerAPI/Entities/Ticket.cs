using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    // TODO: Add required minimum length 5 characters.
    public string Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
