using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Store> Stores { get; set; }
    
    public DbSet<OrderTicket> OrderTickets { get; set; }
    
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    
}