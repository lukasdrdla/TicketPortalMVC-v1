using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {


        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly ITicketService _ticketService;

        public OrderController(IOrderService orderService, IAccountService accountService, ITicketService ticketService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _ticketService = ticketService;
        }



        [HttpGet]
        public async Task<IActionResult> SearchUsers(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            // Předpokládáme, že máte službu pro získání uživatelů (UserService)
            var users = await _accountService.SearchUsersAsync(term);

            // Vracíme seznam uživatelů ve formátu, který očekáváte na frontendové straně
            var result = users.Select(u => new { id = u.Id, text = u.UserName }).ToList();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> SearchTickets(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            // Předpokládáme, že máte službu pro získání eventů (EventService)
            var events = await _ticketService.SearchTicketsAsync(term);

            // Vracíme seznam eventů ve formátu, který očekáváte na frontendové straně
            var result = events.Select(e => new
            {
                id = e.EventId,
                text = e.Event.Name,
                price = e.Price,
                type = e.Type
            }).ToList();
            return Json(result);
        }


        public async Task<IActionResult> OrderList()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        [HttpGet]

        public async Task<IActionResult> OrderCreate()
        {
            var availableTickets = await _ticketService.GetTicketsAsync();
            var model = new OrderViewModel
            {
                AvailableTickets = availableTickets.Select(t => new OrderTicketViewModel
                {
                    TicketId = t.TicketId,
                    EventName = t.Event.Name,
                    Price = t.Price,
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderCreate(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                foreach (var ticket in order.Tickets)
                {
                    var dbTicket = await _ticketService.GetTicketByIdAsync(ticket.TicketId);
                    if (dbTicket == null)
                    {
                        ModelState.AddModelError("", $"Ticket with ID {ticket.TicketId} not found.");
                        return View(order);
                    }

                    ticket.Price = dbTicket.Price; // Aktualizuje cenu z databáze
                }

                var newOrder = new Order
                {
                    UserId = order.UserId,
                    CreatedAt = order.CreatedAt,
                    Total = order.Tickets.Sum(t => t.Price * t.Quantity),
                    IsPaid = order.IsPaid,
                    OrderTickets = order.Tickets.Select(t => new OrderTicket
                    {
                        TicketId = t.TicketId,
                        Quantity = t.Quantity
                    }).ToList()
                };

                await _orderService.CreateOrderAsync(newOrder);
                return RedirectToAction("OrderList");
            }

            return View(order);
        }










        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("OrderList");
        }


        public async Task<IActionResult> OrderDetail(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderDetailViewModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                FirstName = order.User.FirstName,
                LastName = order.User.LastName,
                CreatedAt = order.CreatedAt,
                IsPaid = order.IsPaid,
                Tickets = order.OrderTickets.Select(t => new OrderTicketViewModel
                {
                    TicketId = t.TicketId,
                    Quantity = t.Quantity,
                    Price = t.Ticket.Price,
                    EventName = t.Ticket.Event.Name
                }).ToList()

            };

            return View(model);

        }
        
        
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var success = await _orderService.MarkOrderAsPaidAsync(id);
            if (!success)
            {
                return BadRequest("Chyba při označování objednávky jako zaplacené.");
            }
            return RedirectToAction("OrderDetail", new { id });
        }


    }
    
    
    


}
