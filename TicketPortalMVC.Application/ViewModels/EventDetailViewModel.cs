namespace TicketPortalMVC.Application.ViewModels;

public class EventDetailViewModel
{
    public Domain.Entities.Event Event { get; set; }
    public List<Domain.Entities.Ticket> Tickets { get; set; } = new();
}