using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Web.Models;

namespace TicketPortalMVC.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEventService _eventService;
    
    public HomeController(ILogger<HomeController> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _eventService.GetEventsAsync();
        var topEvents = events.OrderByDescending(e => e.EventRatings.Count > 0 ? e.EventRatings.Average(r => r.Rating) : 0).Take(3)
            .Select(e => new EventViewModel
            {
                EventId = e.EventId,
                Name = e.Name,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                Date = e.Date,
                Capacity = e.Capacity,
                Location = e.Location,
                EventRatings = e.EventRatings
            }).ToList();
        var  eventsThisMonth = events.Where(e => e.Date.Month == DateTime.Now.Month)
            .Select(e => new EventViewModel
            {
                EventId = e.EventId,
                Name = e.Name,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                Date = e.Date,
                Capacity = e.Capacity,
                Location = e.Location,
                EventRatings = e.EventRatings
            }).ToList();
        
        var model = new HomePageViewModel()
        {
            TopEvents = topEvents,
            EventsThisMonth = eventsThisMonth
        };
        
        return View(model);
        
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    
    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}