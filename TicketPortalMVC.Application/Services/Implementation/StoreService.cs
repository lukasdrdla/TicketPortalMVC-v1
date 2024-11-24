using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Domain.Entities;
using TicketPortalMVC.Infrastructure.Data;

namespace TicketPortalMVC.Application.Services.Implementation;

public class StoreService : IStoreService
{
    
    private readonly ApplicationDbContext _context;
    
    public StoreService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Store>> GetStoresAsync()
    {
        var stores = await _context.Stores.ToListAsync();
        return stores;
    }

    public async Task<Store> GetStoreByIdAsync(int id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null)
            throw new Exception("Store not found");
        
        return store;

    }

    public async Task CreateStoreAsync(Store store)
    {
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
    }

    public Task UpdateStoreAsync(Store store)
    {
        _context.Stores.Update(store);
        return _context.SaveChangesAsync();
        
    }

    public async Task DeleteStoreAsync(int id)
    {
        var storeToDelete = await _context.Stores.FindAsync(id);
        
        if (storeToDelete == null)
            throw new Exception("Store not found");
        
        _context.Stores.Remove(storeToDelete);
        await _context.SaveChangesAsync();
        
    }
}