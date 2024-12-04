using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EventController : Controller
    {

        private readonly IEventService _eventService;
        private readonly IEventRatingService _eventRatingService;
        
        public EventController(IEventService eventService, IEventRatingService eventRatingService)
        {
            _eventService = eventService;
            _eventRatingService = eventRatingService;
        }
        
   
        

        /**************************************************** Event Actions ****************************************************/

        public async Task<IActionResult> EventEdit(int id)
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var model = new EventEditViewModel
            {
                EventId = @event.EventId,
                Name = @event.Name,
                Description = @event.Description,
                Location = @event.Location,
                Date = @event.Date,
                Capacity = @event.Capacity,
                ExistingImageUrl = @event.ImageUrl
            };

            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> EventEdit(EventEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingEvent = await _eventService.GetEventByIdAsync(model.EventId);
            if (existingEvent == null)
            {
                return NotFound();
            }

            // Aktualizace polí
            existingEvent.Name = model.Name;
            existingEvent.Description = model.Description;
            existingEvent.Location = model.Location;
            existingEvent.Date = model.Date;
            existingEvent.Capacity = model.Capacity;

            // Pokud byl nahrán nový obrázek
            if (model.Image != null && model.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }


                // Odstranění starého obrázku
                if (!string.IsNullOrEmpty(model.ExistingImageUrl))
                {
                    var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ExistingImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }
                }

                existingEvent.ImageUrl = $"/uploads/{uniqueFileName}";
            }
            

            await _eventService.UpdateEventAsync(existingEvent);
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
        public async Task<IActionResult> EventCreate(EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string imageUrl = string.Empty;

            if (model.Image != null && model.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                imageUrl = $"/uploads/{uniqueFileName}";
            }

            var @event = new Event
            {
                Name = model.Name,
                Description = model.Description,
                Location = model.Location,
                Date = model.Date,
                Capacity = model.Capacity,
                CreatedAt = DateTime.Now,
                ImageUrl = imageUrl
            };

            await _eventService.CreateEventAsync(@event);
            return RedirectToAction("EventList");
        }




    }
}
