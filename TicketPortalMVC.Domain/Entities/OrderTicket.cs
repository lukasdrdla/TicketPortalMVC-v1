namespace TicketPortalMVC.Domain.Entities;

public class OrderTicket
{
    public int OrderTicketId { get; set; }
    public int OrderId { get; set; }
    public int TicketId { get; set; }
    public int Quantity { get; set; }
    
    public Ticket Ticket { get; set; }
    public Order Order { get; set; }
    
}