using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels.Ticket;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;

        public TicketController(ITicketService ticketService, IEventService eventService)
        {
            _ticketService = ticketService;
            _eventService = eventService;
        }


        public async Task<IActionResult> TicketList()
        {
            var tickets = await _ticketService.GetTicketsAsync();
            return View(tickets);
        }

        public async Task<IActionResult> TicketDetail(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> TicketCreate()
        {
            var events = await _eventService.GetEventsAsync(); // Získání událostí

            var model = new TicketViewModel
            {
                Events = events
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> TicketCreate(TicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

            }

            Ticket ticket = new Ticket
            {
                TicketId = model.TicketId,
                EventId = model.EventId,
                Price = model.Price,
                Type = model.Type,
                CreatedAt = model.CreatedAt
            };

            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction("TicketList");
        }



        [HttpPost]
        public async Task<IActionResult> TicketDelete(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction("TicketList");
        }

        public async Task<IActionResult> TicketEdit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            var events = await _eventService.GetEventsAsync();

            if (ticket == null)
            {
                return NotFound(); // Pokud lístek nebyl nalezen, vrať 404
            }

            var model = new TicketEditViewModel
            {
                TicketId = ticket.TicketId,
                EventId = ticket.EventId,
                Price = ticket.Price,
                Type = ticket.Type,
                CreatedAt = ticket.CreatedAt,
                Events = events

            };



            return View(model); 
        }

        public IActionResult Testing()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TicketEdit(TicketEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

            }

            Ticket ticket = new Ticket
            {
                TicketId = model.TicketId,
                EventId = model.EventId,
                Price = model.Price,
                Type = model.Type,
                CreatedAt = model.CreatedAt
            };

            await _ticketService.UpdateTicketAsync(ticket);
            return RedirectToAction("TicketList");
        }

    }
}
