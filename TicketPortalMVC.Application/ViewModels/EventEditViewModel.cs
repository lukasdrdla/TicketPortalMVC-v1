using Microsoft.AspNetCore.Http;

namespace TicketPortalMVC.Application.ViewModels;

public class EventEditViewModel
{
    public int EventId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public string? ExistingImageUrl { get; set; } // URL aktuálního obrázku
    public IFormFile? Image { get; set; } // Nový obrázek
}