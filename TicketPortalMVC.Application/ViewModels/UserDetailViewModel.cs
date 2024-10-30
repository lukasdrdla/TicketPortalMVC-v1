using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class UserDetailViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
