using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Repositories;

public class TicketRepository(
    List<Ticket> _tickets)
{
    public List<Ticket> GetAllTickets()
    {
        return _tickets;
    }

    public Ticket? GetTicketById(Guid id)
    {
        return _tickets.FirstOrDefault(t => t.Id == id);
    }

    public void AddTicket(Ticket ticket)
    {
        _tickets.Add(ticket);
    }
}
