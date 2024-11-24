namespace TicketPortalMVC.Domain.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public int EventId { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Event Event { get; set; }
    public List<OrderTicket> OrderTickets { get; set; } = new();
    
}