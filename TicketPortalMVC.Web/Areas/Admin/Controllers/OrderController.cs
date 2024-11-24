using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Index()
        {
            return View();
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
        
        public IActionResult OrderCreate()
        {
            var model = new OrderViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderCreate(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(model);
            }


            var user = await _accountService.GetUserByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError("UserId", "User not found");
                return View(model);
            }

            var order = new Order
            {
                UserId = model.UserId,
                Total = model.TotalPrice,
                CreatedAt = DateTime.Now
            };
            
            List<OrderTicket> orderTickets = new List<OrderTicket>();
            if (!string.IsNullOrEmpty(model.OrderTicketsJson))
            {
                try
                {
                    orderTickets = JsonConvert.DeserializeObject<List<OrderTicket>>(model.OrderTicketsJson);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("OrderTicketsJson", "Invalid data format");
                    return View(model);
                }
            }
            
            foreach (var orderTicket in orderTickets)
            {
                var ticket = await _ticketService.GetTicketByIdAsync(orderTicket.TicketId);
                if (ticket == null)
                {
                    ModelState.AddModelError("OrderTicketsJson", "Ticket not found");
                    return View(model);
                }
                order.OrderTickets.Add(new OrderTicket
                {
                    TicketId = orderTicket.TicketId,
                    Quantity = orderTicket.Quantity,
                });
            }
            
            
            

            // Uložení objednávky do databáze
            try
            {
                await _orderService.CreateOrderAsync(order);
            }
            catch (Exception ex)
            {
                // Pokud nastane chyba při ukládání, přidejte chybu do ModelState
                ModelState.AddModelError(string.Empty, "Failed to create order: " + ex.Message);
                return View(model);
            }
            return RedirectToAction("OrderList");
        }

        
        
        
        
        public async Task<IActionResult> OrderDetail(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            
            var model = new OrderViewModel
            {
                OrderId = order.OrderId,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.Total,
                OrderTickets = order.OrderTickets
            };

            
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("OrderList");
        }
        
        



}
}
