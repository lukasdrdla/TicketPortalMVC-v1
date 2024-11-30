using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class OrderEditViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal Total => Tickets.Sum(x => x.Price * x.Quantity);
        public bool IsPaid { get; set; }

        public List<OrderTicketViewModel> Tickets { get; set; } = new();
        public List<UserViewModel> AvailableUsers { get; set; } = new();
        
        public List<OrderTicketViewModel> AvailableTickets { get; set; } = new();
        

 
        
        
    }
    


}