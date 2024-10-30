using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels.Ticket
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Event is required.")]
        public int EventId { get; set; }  // ID vybrané události

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        public string Type { get; set; }  // Typ lístku (např. "VIP", "Standard")

        [Required(ErrorMessage = "Created At is required.")]
        public DateTime CreatedAt { get; set; }

        // Seznam všech událostí pro rozbalovací menu
        public List<Event> Events { get; set; } // Seznam všech událostí pro rozbalovací menu
    }
}
