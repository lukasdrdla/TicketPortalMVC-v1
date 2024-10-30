using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels.Order;

public class OrderDetailViewModel
{
    public Domain.Entities.Order Order { get; set; }
    public List<OrderTicket> OrderTicket { get; set; }
    public User User { get; set; }
}