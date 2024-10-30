using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels.Ticket;

public class TicketCreateViewModel
{
    public int TicketId { get; set; }

    [Required(ErrorMessage = "Event is required.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public decimal Price { get; set; }

    public string Type { get; set; } // Zvaž přidání [Required] pokud je to potřeba

    [Required(ErrorMessage = "Created At is required.")]
    public DateTime CreatedAt { get; set; }

    public List<Event> Events { get; set; }
}
