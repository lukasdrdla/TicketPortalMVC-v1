namespace TicketPortalMVC.Application.ViewModels;

public class HomePageViewModel
{
    public List<EventViewModel> TopEvents { get; set; }
    public List<EventViewModel> EventsThisMonth { get; set; }
}