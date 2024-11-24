using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.ViewModels;

public class EventViewModel
{
    public int EventId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }
    
    public ICollection<EventRating> EventRatings { get; set; }
    public double AverageRating => EventRatings.Count > 0 ? EventRatings.Average(x => x.Rating) : 0;
    
}