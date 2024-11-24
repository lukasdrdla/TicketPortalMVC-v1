
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(int id);

    Task<int> GetTotalOrdersAsync();


    Task<List<UserOrderViewModel>> GetUserOrdersAsync(string userId);
    Task<Order> CreateOrderFromCart(string userId);
}