using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface ICartService
{
    Task <List<CartItem>> GetCartItems(string userId);
    Task AddToCart(string userId, int ticketId, int quantity);
    Task RemoveFromCart(string userId, int ticketId);

    Task<int> GetCartItemCount(string userId);

}