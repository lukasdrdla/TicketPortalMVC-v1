namespace TicketPortalMVC.Domain.Entities;

public class CartItem
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int TicketId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Cart Cart { get; set; }
    public Ticket Ticket { get; set; }
}