using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public List<string> Roles { get; set; } = new();  // Seznam rolí uživatele

    }
}
