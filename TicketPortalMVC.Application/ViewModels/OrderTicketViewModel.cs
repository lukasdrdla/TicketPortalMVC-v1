namespace TicketPortalMVC.Application.ViewModels;

public class OrderTicketViewModel
{
    public int TicketId { get; set; }
    public int Quantity { get; set; }
    
    public string EventName { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
}