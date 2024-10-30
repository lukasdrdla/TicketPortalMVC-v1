using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;

namespace TicketPortalMVC.Web.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;
    
    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    [HttpGet("/udalosti")]
    public IActionResult Index(string? searchTerms, DateTime? startDate, DateTime? endDate, string? location)
    {
        
        var events = _eventService.GetEventsAsync().Result;
        
        if (!string.IsNullOrEmpty(searchTerms))
        {
            events = events.Where(e => e.Name.Contains(searchTerms, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (startDate.HasValue)
            events = events.Where(e => e.Date >= startDate.Value).ToList();

        if (endDate.HasValue)
            events = events.Where(e => e.Date <= endDate.Value).ToList();

        if (!string.IsNullOrEmpty(location))
            events = events.Where(e => e.Location.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();
        
        var viewModel = new EventFilterViewModel
        {
            SearchTerms = searchTerms,
            StartDate = startDate,
            EndDate = endDate,
            Location = location,
            Events = events
        };
        
        return View(viewModel);
    }
    
    [HttpGet("events/event-detail/{id}")]
    public IActionResult EventDetail(int id)
    {
        var @event = _eventService.GetEventByIdAsync(id).Result;
        return View(@event);
    }
    
}