using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public List<Notification>? Notifications { get; set; }
}
