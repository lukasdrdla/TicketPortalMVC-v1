
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(int id);

    Task<int> GetTotalOrdersAsync();
    

    Task AddTicketToOrderAsync(int orderId, int ticketId, int quantity);
    Task RemoveTicketFromOrderAsync(int orderId, int ticketId);

    Task<List<UserOrderViewModel>> GetUserOrdersAsync(string userId);
    Task<Order> CreateOrderFromCart(string userId);
    
    Task<bool> MarkOrderAsPaidAsync(int orderId);
    Task<List<decimal>> GetTotalRevenueAsync();
}