using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;
    
    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Ticket>> GetTicketsAsync()
    {
        var tickets = await _context.Tickets
            .Include(x => x.Event)
            .ToListAsync();
        return tickets;
    }

    public async Task<Ticket> GetTicketByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(x => x.Event)
            .FirstOrDefaultAsync(x => x.TicketId == id);
        if (ticket == null)
        {
            throw new Exception("Ticket not found");
        }
        return ticket;
        
    }

    public async Task CreateTicketAsync(Ticket ticket)
    {
        try
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ticket", ex);
        }

    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {

        var existingTicket = await _context.Tickets.FindAsync(ticket.TicketId);
        if (existingTicket == null)
        {
            throw new InvalidOperationException("Ticket not found.");
        }

        // Aktualizovat hodnoty existujícího tiketu
        existingTicket.EventId = ticket.EventId;
        existingTicket.Price = ticket.Price;
        existingTicket.Type = ticket.Type;
        existingTicket.CreatedAt = ticket.CreatedAt;

        try
        {
            await _context.SaveChangesAsync(); // Uložení změn
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update ticket", ex);
        }
    }


    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            throw new Exception("Ticket not found");
        }
        
        try
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete ticket", ex);
        }
        
    }

    public async Task<int> GetTotalTicketsAsync()
    {
        var totalTickets = await _context.Tickets.CountAsync();
        return totalTickets;

    }

    public async Task<List<Ticket>> SearchTicketsAsync(string term)
    {
        return await _context.Tickets
            .Include(x => x.Event)
            .Where(x => x.Type.Contains(term) || x.Event.Name.Contains(term))
            .ToListAsync();
    }
}