using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class OrderViewModel
    {
        public Domain.Entities.Order Order { get; set; }
        public List<OrderTicket> OrderTicket { get; set; }
        public User User { get; set; }
    }
}
