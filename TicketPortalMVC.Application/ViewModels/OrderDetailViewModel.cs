using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels;

public class OrderDetailViewModel
{
    public int OrderId { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public decimal Total => Tickets.Sum(x => x.Price * x.Quantity);
    public bool IsPaid { get; set; }
    
    
    
    public User User { get; set; }
    public List<OrderTicketViewModel> Tickets { get; set; } = new List<OrderTicketViewModel>();
}