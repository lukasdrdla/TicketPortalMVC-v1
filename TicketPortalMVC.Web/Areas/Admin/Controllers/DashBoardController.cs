﻿using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        private readonly IOrderService _orderService;


        public DashBoardController(ITicketService ticketService, IEventService eventService, IOrderService orderService)
        {
            _ticketService = ticketService;
            _eventService = eventService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var totalRevenue = await _orderService.GetTotalRevenueAsync();
            var model = new DashBoardViewModel
            {
                TotalTickets = await _ticketService.GetTotalTicketsAsync(),
                TotalCustomers = 20,
                TotalEvents = await _eventService.GetTotalEventsAsync(),
                TotalOrders = await _orderService.GetTotalOrdersAsync(),
                Orders = await _orderService.GetOrdersAsync(),
                Months = new List<string> { "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec" },
                Revenues = totalRevenue
            };

            return View(model);
        }
    }
}
