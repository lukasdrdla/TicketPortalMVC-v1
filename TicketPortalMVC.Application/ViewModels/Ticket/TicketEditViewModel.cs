using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels.Ticket
{
    public class TicketEditViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Event is required.")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        public string Type { get; set; } // Zvaž přidání [Required] pokud je to potřeba

        [Required(ErrorMessage = "Created At is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Event> Events { get; set; }

    }
}
