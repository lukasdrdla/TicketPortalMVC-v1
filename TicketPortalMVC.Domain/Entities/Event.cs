namespace TicketPortalMVC.Domain.Entities;

public class Event
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<Ticket> Tickets { get; set; } = new();
    public ICollection<EventRating> EventRatings { get; set; }
    public double AverageRating => EventRatings.Count > 0 ? EventRatings.Average(x => x.Rating) : 0;
}