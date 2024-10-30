namespace TicketPortalMVC.Domain.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public bool IsPaid { get; set; }

    public User User { get; set; }
    public List<OrderTicket> OrderTickets { get; set; } = new();
    
}