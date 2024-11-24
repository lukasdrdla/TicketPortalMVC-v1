namespace TicketPortalMVC.Domain.Entities;

public class EventRating
{
    public int EventRatingId { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; } 
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public Event Event { get; set; }
    public User User { get; set; }
    
}