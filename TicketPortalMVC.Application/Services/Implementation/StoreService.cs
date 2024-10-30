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

    public Task CreateStoreAsync(Store store)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStoreAsync(Store store)
    {
        throw new NotImplementedException();
    }

    public Task DeleteStoreAsync(int id)
    {
        throw new NotImplementedException();
    }
}