using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; } // User Id
        public string OrderTicketsJson { get; set; } // Serializovaná data pro vstupenky


        public List<OrderTicket> OrderTickets { get; set; }
    }
    


}
