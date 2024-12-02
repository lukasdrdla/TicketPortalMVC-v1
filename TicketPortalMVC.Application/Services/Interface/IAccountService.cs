using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IAccountService
{
    Task RegisterAsync(RegisterViewModel registerViewModel);
    Task LoginAsync(LoginViewModel loginViewModel);
    Task LogoutAsync();

    Task<User> GetCurrentUserAsync(ClaimsPrincipal principal);

    
    // Admin methods
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(string id);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    
    
    
    
    // Extra methods
    Task<List<User>> SearchUsersAsync(string term);
    Task<int> GetUsersCountAsync();
}