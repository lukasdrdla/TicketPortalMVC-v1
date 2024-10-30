using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    
    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Event>> GetEventsAsync()
    {
        var events = await _context.Events
            .Include(e => e.Tickets)
            .ToListAsync();
        return events;
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        var @event = await _context.Events
            .Include(e => e.Tickets) // Načtení souvisejících lístků
            .FirstOrDefaultAsync(e => e.EventId == id); // Hledáme podle EventId

        if (@event == null)
        {
            throw new Exception("Event not found");
        }
        
        return @event;

    }

    public async Task CreateEventAsync(Event @event)
    {
        if (@event == null)
        {
            throw new Exception("Event is null");
        }

        try
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating event", ex);
        }
        
        
    }

    public async Task UpdateEventAsync(Event @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event), "Ticket cannot be null.");
        }
        
        var existingEvent = await _context.Events.FindAsync(@event.EventId);
        if (existingEvent == null)
        {
            throw new Exception("Event not found");
        }
        
        existingEvent.Name = @event.Name;
        existingEvent.Description = @event.Description;
        existingEvent.ImageUrl = @event.ImageUrl;
        existingEvent.Location = @event.Location;
        existingEvent.Date = @event.Date;
        existingEvent.Capacity = @event.Capacity;
        existingEvent.CreatedAt = @event.CreatedAt;

        try
        {
            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating event", ex);
        }
        
        
    }

    public async Task DeleteEventAsync(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Event not found");
        }
        
    }

    public async Task<int> GetTotalEventsAsync()
    {
        var totalEvents = await _context.Events.CountAsync();
        return totalEvents;
    }
}