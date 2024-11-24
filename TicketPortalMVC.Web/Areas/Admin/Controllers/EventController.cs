using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {

        private readonly IEventService _eventService;
        private readonly IEventRatingService _eventRatingService;
        private readonly ITicketService _ticketService;
        
        public EventController(IEventService eventService, IEventRatingService eventRatingService, ITicketService ticketService)
        {
            _eventService = eventService;
            _eventRatingService = eventRatingService;
            _ticketService = ticketService;
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
            var eventDetail = await _eventService.GetEventByIdAsync(id);
            var eventRatings = await _eventRatingService.GetRatingsAsync(id);
            var eventDetailViewModel = new EventDetailViewModel
            {
                Event = eventDetail,
                EventRatings = eventRatings
            };
            
            return View(eventDetailViewModel);

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
