using TicketTrackerAPI.Entities;

namespace TicketTrackerAPI.Repositories;

public interface ITicketInMemoryRepository
{
    List<Ticket> GetAllTickets();
    Ticket? GetTicketById(Guid id);
    void AddTicket(Ticket ticket);
}
