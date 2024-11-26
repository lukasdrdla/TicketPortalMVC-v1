﻿using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IAccountService
{
    Task RegisterAsync(RegisterViewModel registerViewModel);
    Task LoginAsync(LoginViewModel loginViewModel);
    Task LogoutAsync();

    
    // Admin methods
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(string id);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    
    
    // Extra methods
    Task<List<User>> SearchUsersAsync(string term);
    Task<int> GetUsersCountAsync();
}