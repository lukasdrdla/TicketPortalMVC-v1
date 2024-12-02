using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;

namespace TicketPortalMVC.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    
    public CartController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }
    
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var cartItems = await _cartService.GetCartItems(userId);
        
        var itemCount = await _cartService.GetCartItemCount(userId);
        ViewBag.ItemCount = itemCount;
        
        return View(cartItems);

    }
    
    [HttpPost]
    public async Task<IActionResult> AddToCart(int ticketId, int quantity)
    {
        // Získání ID přihlášeného uživatele
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId != null)
        {
            // Přidání položky do košíku
            await _cartService.AddToCart(userId, ticketId, quantity);
            
            // Přesměrování zpět na stránku košíku nebo na jinou stránku
            return RedirectToAction("Index");
        }
        
        // Pokud uživatel není přihlášený, přesměrujeme ho na přihlašovací stránku
        return RedirectToAction("Login", "Account");
        
        
    }
    
    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var cartItems = await _cartService.GetCartItems(userId);
        
        if (cartItems.Count == 0)
        {
            TempData["Error"] = "Tvůj košík je prázdný.";
            return RedirectToAction("Index");
        }
        
        var itemCount = await _cartService.GetCartItemCount(userId);
        ViewBag.ItemCount = itemCount;
        
        return View(cartItems);
    }
    
    [HttpPost]

    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Confirm()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            // Vytvoření objednávky z košíku
            var order = await _orderService.CreateOrderFromCart(userId);
            
            TempData["OrderId"] = order.OrderId;
            
            // Přesměrování na potvrzení objednávky
            return RedirectToAction("Confirmation");
        }
        catch (InvalidOperationException ex)
        {
            // Pokud nastala chyba, např. prázdný košík, zobrazíme zprávu
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
    
    [Authorize]
    public async Task<IActionResult> Confirmation()
    {
        if (!TempData.TryGetValue("OrderId", out var orderId))
        {
            return RedirectToAction("Index");
        }
        var order = await _orderService.GetOrderByIdAsync((int)orderId);
        return View(order);
    }
    


    [HttpPost]
    public IActionResult UpdateQuantity(int cartItemId, int quantity)
    {
        // Získání položky z košíku
        var cartItem = _cartService.GetCartItemById(cartItemId);
        if (cartItem == null)
        {
            TempData["Error"] = "Položka nenalezena.";
            return RedirectToAction("Index"); // Nebo jiná stránka
        }

        // Aktualizace množství
        cartItem.Result.Quantity = quantity;
        _cartService.UpdateCartItem(cartItem.Result);

        return RedirectToAction("Index"); // Nebo přesměrování na košík
    }


    [HttpPost]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        // Odebrání položky z košíku
        _cartService.RemoveFromCart(cartItemId);

        return RedirectToAction("Index"); // Nebo jiná stránka
    }



    
    
}