using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;

        public EventController(ITicketService ticketService, IEventService eventService)
        {
            _ticketService = ticketService;
            _eventService = eventService;
        }

        /**************************************************** Event Actions ****************************************************/

        public async Task<IActionResult> EventEdit(int id)
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            return View(@event);
        }

        [HttpPost]
        public async Task<IActionResult> EventEdit(Event @event)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(@event);
            }



            await _eventService.UpdateEventAsync(@event);
            return RedirectToAction("EventList");
        }

        [HttpPost]
        public async Task<IActionResult> EventDelete(int id)
        {
            


            await _eventService.DeleteEventAsync(id);

            return RedirectToAction("EventList", "Event", new { area = "Admin" }); // Správná redirekce
        }


        public async Task<IActionResult> EventList()
        {
            var events = await _eventService.GetEventsAsync();
            return View(events);
        }

        public async Task<IActionResult> EventDetail(int id)
        {
            EventDetailViewModel model = new EventDetailViewModel
            {
                Event = await _eventService.GetEventByIdAsync(id),
                Tickets = await _ticketService.GetTicketsAsync()
            };

            return View(model);

        }

        public IActionResult EventCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EventCreate(Event @event)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(@event);
            }

            await _eventService.CreateEventAsync(@event);
            return RedirectToAction("EventList");
        }
    }
}
