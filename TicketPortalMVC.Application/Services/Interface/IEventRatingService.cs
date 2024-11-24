using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IEventRatingService
{
    //add rating to event with returns to model state
    Task AddRatingAsync(int eventId, int rating, string comment);
    Task<List<EventRating>> GetRatingsAsync(int eventId);
    Task<double> GetAverageRatingAsync(int eventId);
}