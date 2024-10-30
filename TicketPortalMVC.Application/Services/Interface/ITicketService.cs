using TicketPortalMVC.Application.ViewModels.Ticket;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface ITicketService
{
    Task<List<Ticket>> GetTicketsAsync();
    Task<Ticket> GetTicketByIdAsync(int id);
    Task CreateTicketAsync(Ticket ticket);
    Task UpdateTicketAsync(Ticket ticket);
    Task DeleteTicketAsync(int id);

    Task<int> GetTotalTicketsAsync();
}