using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Repositories;

public class TicketInMemoryRepository : ITicketInMemoryRepository
{
    List<Ticket> _tickets = new List<Ticket>();

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
