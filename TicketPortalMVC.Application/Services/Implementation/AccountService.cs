using Microsoft.AspNetCore.Http;
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
            throw new InvalidOperationException("User with this email already exists.");
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
            throw new InvalidOperationException("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "USER");
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException("Failed to assign 'USER' role.");
        }
        

    }


    public async Task LoginAsync(LoginViewModel loginViewModel)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!passwordValid)
            {
                throw new Exception("Invalid password");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to sign in");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during login", ex);
        }
    }
    
    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
    
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return user;
    }


    public Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
       
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("An error occurred during user deletion.");
        }
    }
    


    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<List<User>> SearchUsersAsync(string term)
    {
        return await _userManager.Users.Where(u => u.Email.Contains(term)).ToListAsync();
    }
}