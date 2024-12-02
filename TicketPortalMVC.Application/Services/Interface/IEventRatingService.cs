using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IEventRatingService
{
    Task AddRatingAsync(int eventId, int rating, string comment);
    Task<List<EventRating>> GetRatingsAsync(int eventId);
}