using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class OrderViewModel
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal Total => Tickets.Sum(x => x.Price * x.Quantity);
        public bool IsPaid { get; set; }
        
        public List<OrderTicketViewModel> Tickets { get; set; } = new List<OrderTicketViewModel>();
        
        public List<OrderTicketViewModel> AvailableTickets { get; set; } = new List<OrderTicketViewModel>();
        

 
        
        
    }
    


}