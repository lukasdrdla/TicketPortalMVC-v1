namespace TicketPortalMVC.Domain.Entities;

public class Cart
{
    public int CartId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public User User { get; set; }
    public List<CartItem> CartItems { get; set; }
}