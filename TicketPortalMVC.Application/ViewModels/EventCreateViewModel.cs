using Microsoft.AspNetCore.Http;

namespace TicketPortalMVC.Application.ViewModels;

public class EventCreateViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public IFormFile Image { get; set; }

}