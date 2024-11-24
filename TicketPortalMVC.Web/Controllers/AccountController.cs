using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Controllers;

public class AccountController : Controller
{
    
    private readonly IAccountService _accountService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IOrderService _orderService;
    
    public AccountController(IAccountService accountService, SignInManager<User> signInManager, UserManager<User> userManager, IOrderService orderService)
    {
        _accountService = accountService;
        _signInManager = signInManager;
        _userManager = userManager;
        _orderService = orderService;
    }
    
    
    
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }
        
        try
        {
            await _accountService.RegisterAsync(registerViewModel);
            return RedirectToAction("RegistrationSuccess", "Account");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(registerViewModel);
        }
    }
    
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        try
        {
            await _accountService.LoginAsync(loginViewModel);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(loginViewModel);
        }
        
        
    }
    
    
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }
    
    public async Task<IActionResult> UserOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        var orders = await _orderService.GetUserOrdersAsync(user.Id);
        return View(orders);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Profile(User user)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Address = user.Address;
        currentUser.City = user.City;
        currentUser.Country = user.Country;
        currentUser.PostalCode = user.PostalCode;
        currentUser.PhoneNumber = user.PhoneNumber;
        
        var result = await _userManager.UpdateAsync(currentUser);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Failed to update user");
            return View(currentUser);
        }
        else
        {
            TempData["SuccessMessage"] = "Profile updated successfully";
        }
        
        
        return View(currentUser);
    }
    
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Index", "Events");
    }
    
    public IActionResult RegistrationSuccess()
    {
        return View();
    }


}