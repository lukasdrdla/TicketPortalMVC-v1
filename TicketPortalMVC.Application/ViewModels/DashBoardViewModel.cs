using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketPortalMVC.Application.ViewModels
{
    public class DashBoardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }
        public int TotalEvents { get; set; }


        public List<Domain.Entities.Order> Orders { get; set; }

    }
}
