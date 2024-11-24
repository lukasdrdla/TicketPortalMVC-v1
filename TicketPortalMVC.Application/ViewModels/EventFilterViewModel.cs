namespace TicketPortalMVC.Application.ViewModels;

public class EventFilterViewModel
{
    public string? SearchTerms { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public List<Domain.Entities.Event> Events { get; set; } = new ();
}