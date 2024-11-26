using Microsoft.EntityFrameworkCore;

using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    
    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Order>> GetOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .ToListAsync();
        return orders;
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderTickets)         // Načteme položky objednávky
            .ThenInclude(ot => ot.Ticket)
            .ThenInclude(t => t.Event)            // Načteme události
            .Include(o => o.User)                 // Načteme uživatele
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        return order;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        if (order == null)
        {
            throw new Exception("Order is null");
        }

        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create order", ex);
        }

        return order;
    }


    public async Task UpdateOrderAsync(Order order)
    {
        if (order == null)
        {
            throw new Exception("Order is null");
        }
        
        var existingOrder = await _context.Orders.FindAsync(order.OrderId);
        if (existingOrder == null)
        {
            throw new Exception("Order not found");
        }
        
        existingOrder.UserId = order.UserId;
        existingOrder.Total = order.Total;
        existingOrder.CreatedAt = order.CreatedAt;
        existingOrder.IsPaid = order.IsPaid;
        

        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update order", ex);
        }
        
    }

    public async Task DeleteOrderAsync(int id)
    {
        
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        try
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete order", ex);
        }
        
        
    }

    public async Task<List<UserOrderViewModel>> GetUserOrdersAsync(string userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Select(o => new UserOrderViewModel
            {
                OrderId = o.OrderId,
                EventName = o.OrderTickets.FirstOrDefault() != null ? o.OrderTickets.FirstOrDefault().Ticket.Event.Name : "Unknown Event",
                Price = o.OrderTickets.FirstOrDefault() != null ? o.OrderTickets.FirstOrDefault().Ticket.Price : 0,
                Quantity = o.OrderTickets.Any() ? o.OrderTickets.Sum(ot => ot.Quantity) : 0
            }).ToListAsync();
    }


    public async Task<Order> CreateOrderFromCart(string userId)
    {
        // Načteme aktivní košík uživatele
        var cart = await _context.Cart
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Ticket)
            .ThenInclude(t => t.Event) // Ensure Event is included
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

        if (cart == null || !cart.CartItems.Any())
        {
            throw new InvalidOperationException("Košík je prázdný nebo neexistuje.");
        }

        // Vytvoříme novou objednávku
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Total = cart.CartItems.Sum(ci => ci.Quantity * ci.Price),
            OrderTickets = new List<OrderTicket>()
        };

        // Přeneseme položky z košíku do objednávky
        foreach (var cartItem in cart.CartItems)
        {
            var orderTicket = new OrderTicket
            {
                TicketId = cartItem.TicketId,
                Quantity = cartItem.Quantity,
                        
            };
            order.OrderTickets.Add(orderTicket);
        }

        // Přidáme objednávku do databáze
        _context.Orders.Add(order);

        // Deaktivujeme košík (nebo vyprázdníme)
        cart.IsActive = false;
        _context.Cart.Update(cart);

        // Uložíme změny
        await _context.SaveChangesAsync();

        return order;    
    }

    public async Task<bool> MarkOrderAsPaidAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        
        order.IsPaid = true;
        
        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to mark order as paid", ex);
        }
        
    }

    public async Task<List<decimal>> GetTotalRevenueAsync()
    {
        // Načtení tržeb seskupených podle měsíce
        var revenues = await _context.Orders
            .Where(o => o.IsPaid)
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new 
            {
                Month = g.Key.Month,
                Total = g.Sum(o => o.Total)
            })
            .ToListAsync();

        // Vytvoření seznamu všech měsíců (1–12) s výchozí hodnotou 0
        var fullRevenues = Enumerable.Range(1, 12)
            .Select(month => revenues.FirstOrDefault(r => r.Month == month)?.Total ?? 0m)
            .ToList();

        return fullRevenues;
    }

    


    public async Task<int> GetTotalOrdersAsync()
    {
        var totalOrders = await _context.Orders.CountAsync();
        return totalOrders;
    }
}