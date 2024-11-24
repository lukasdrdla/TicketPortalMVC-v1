using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class EventRatingService : IEventRatingService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public EventRatingService(ApplicationDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task AddRatingAsync(int eventId, int rating, string comment)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        
        if (user == null)
        {
            throw new InvalidOperationException("Aby jsi mohl hodnotit události, musíš být přihlášený.");
        }
        
        var existingRating = await _context.EventRatings.FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == user.Id);
        if (existingRating != null)
        {
            throw new InvalidOperationException("Už jsi tuto událost hodnotil.");
            
        }
        
        
        var eventRating = new EventRating
        {
            EventId = eventId,
            UserId = user.Id,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };
        
        try
        {
            await _context.EventRatings.AddAsync(eventRating);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to add rating: " + ex.Message);
        }
    }

    public async Task<List<EventRating>> GetRatingsAsync(int eventId)
    {
        
        var ratings = await _context.EventRatings.Where(r => r.EventId == eventId)
            .Include(r => r.User)
            .ToListAsync();
        return ratings;
    }

    public async Task<double> GetAverageRatingAsync(int eventId)
    {
        var ratings = await GetRatingsAsync(eventId);
        return ratings.Count > 0 ? ratings.Average(x => x.Rating) : 0;
        
    }
}