using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
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
        return View(events);
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