using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels;

public class EventDetailViewModel
{
    public Event Event { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
    
    public string UserId { get; set; } = string.Empty;
    public ICollection<EventRating> EventRatings { get; set; }
    public double AverageRating => EventRatings.Count > 0 ? EventRatings.Average(x => x.Rating) : 0;
}