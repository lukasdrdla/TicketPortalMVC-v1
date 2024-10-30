using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IAccountService
{
    Task RegisterAsync(RegisterViewModel registerViewModel);
    Task LoginAsync(LoginViewModel loginViewModel);
    
    Task<User> GetUserByEmailAsync(string email);
    //all methods below are for admin
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(string id);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    
    Task LogoutAsync();
}