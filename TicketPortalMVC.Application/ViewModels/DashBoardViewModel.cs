
namespace TicketPortalMVC.Application.ViewModels
{
    public class DashBoardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }
        public int TotalEvents { get; set; }


        public List<Domain.Entities.Order> Orders { get; set; }
        
        public List<string> Months { get; set; } = new List<string>();
        public List<decimal> Revenues { get; set; } = new List<decimal>();
        

    }
}
