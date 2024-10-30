using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task RegisterAsync(RegisterViewModel registerViewModel)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }
        
        var user = new User
        {
            Email = registerViewModel.Email,
            UserName = registerViewModel.Email,
            FirstName = registerViewModel.FirstName,
            LastName = registerViewModel.LastName,
            Address = registerViewModel.Address,
            City = registerViewModel.City,
            Country = registerViewModel.Country,
            PostalCode = registerViewModel.PostalCode,
            PhoneNumber = registerViewModel.PhoneNumber
        };
        
        var result = await _userManager.CreateAsync(user, registerViewModel.Password);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create user");
        }
    }

    public async Task LoginAsync(LoginViewModel loginViewModel)
    {
        // Vyhledáme uživatele podle e-mailu
        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Zkontrolujeme správnost hesla
        var passwordValid = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        if (!passwordValid)
        {
            throw new Exception("Invalid password");
        }

        // Přihlášení uživatele
        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to sign in");
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return user;
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users;
    }

    public Task<User> GetUserByIdAsync(string id)
    {
        var user = _userManager.FindByIdAsync(id);
        if (user != null)
        {
            return user;
        }
        else
        {
            throw new Exception("User not found");
        }
    }

    public Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
       
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}