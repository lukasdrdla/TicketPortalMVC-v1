using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;

namespace TicketPortalMVC.Web.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;
    private readonly IEventRatingService _eventRatingService;
    
    public EventsController(IEventService eventService, IEventRatingService eventRatingService)
    {
        _eventService = eventService;
        _eventRatingService = eventRatingService;
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
    public async Task<IActionResult> EventDetail(int id)
    {
        var @event = await _eventService.GetEventByIdAsync(id);
        ViewData["EventId"] = id;
        
        return View(@event);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRating(int eventId, int rating, string comment)
    {
        comment ??= string.Empty;
        
        try
        {
            await _eventRatingService.AddRatingAsync(eventId, rating, comment);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("EventDetail", new { id = eventId });
        }
    
        TempData["SuccessMessage"] = "Your rating has been successfully submitted.";
        
        return RedirectToAction("EventDetail", new { id = eventId });
    }
    
    [HttpGet]
    public IActionResult Search(string searchTerms)
    {
        var events = _eventService.GetEventsAsync().Result;
        events = events.Where(e => e.Name.Contains(searchTerms, StringComparison.OrdinalIgnoreCase)).ToList();
        return Json(events);
    }
  
    
}