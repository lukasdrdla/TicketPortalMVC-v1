using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {


        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;



        public OrderController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderList()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(order.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var orderDetailViewModel = new OrderViewModel
            {
                Order = order,
                User = user,
                OrderTicket = order.OrderTickets.ToList()
            };

            return View(orderDetailViewModel);


        }


    }
}
