using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CartItem>> GetCartItems(string userId)
    {
        var cart = await _context.Cart
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Ticket)
            .ThenInclude(t => t.Event)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

        return cart?.CartItems.ToList() ?? new List<CartItem>();
 
    }

    public async Task AddToCart(string userId, int ticketId, int quantity)
    {
        var cart = await _context.Cart
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
        
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CartItems = new List<CartItem>()
            };
            _context.Cart.Add(cart);
        }

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.TicketId == ticketId);
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                TicketId = ticketId,
                Quantity = quantity,
                Price = (await _context.Tickets.FindAsync(ticketId)).Price
            };
            cart.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _context.SaveChangesAsync();
    }

    public Task RemoveFromCart(int cardItemId)
    {
        var cartItem = _context.CartItems.Find(cardItemId);
        if (cartItem == null)
        {
            throw new Exception("CartItem not found");
        }
        
        _context.CartItems.Remove(cartItem);
        return _context.SaveChangesAsync();
        
        
    }

    public async Task<int> GetCartItemCount(string userId)
    {
        var cart = await _context.Cart
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

        if (cart == null)
        {
            return 0;
        }

        return cart.CartItems.Sum(ci => ci.Quantity);
    }

    public async Task<CartItem> GetCartItemById(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        return item;
    }

    public async Task UpdateCartItem(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }
}