namespace TicketPortalMVC.Application.ViewModels;

public class UserOrderViewModel
{
    public int OrderId { get; set; }
    public string EventName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
}