using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IStoreService
{
    Task<List<Store>> GetStoresAsync();
    Task<Store> GetStoreByIdAsync(int id);
    Task CreateStoreAsync(Store store);
    Task UpdateStoreAsync(Store store);
    Task DeleteStoreAsync(int id);
}