using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers;
[Area("Admin")]

public class StoreController : Controller
{
    private readonly IStoreService _storeService;
    
    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }
    
    public async Task<IActionResult> StoreList()
    {
        var stores = await _storeService.GetStoresAsync();
        return View(stores);
    }
    
    
    public IActionResult StoreCreate()
    {
        return View();
    }

    public async Task<IActionResult> StoreDetail(int id)
    {
        var store = await _storeService.GetStoreByIdAsync(id);
        return View(store);
    }
    
    public async Task<IActionResult> StoreEdit(int id)
    {
        var store = await _storeService.GetStoreByIdAsync(id);
        
        var storeViewModel = new StoreViewModel
        {
            StoreId = store.StoreId,
            Name = store.Name,
            Address = store.Address,
            City = store.City,
            Phone = store.Phone,
            OpeningHours = store.OpeningHours
        };
        
        return View(storeViewModel);
        
    }
    
    // Post
    
    [HttpPost]
    public async Task<IActionResult> StoreDelete(int id)
    {
        await _storeService.DeleteStoreAsync(id);
        return RedirectToAction("StoreList");
    }
    
    
    [HttpPost]
    public async Task<IActionResult> StoreCreate(StoreViewModel store)
    {
        var newStore = new Store
        {
            Name = store.Name,
            Address = store.Address,
            City = store.City,
            Phone = store.Phone,
            OpeningHours = store.OpeningHours
        };
        
        await _storeService.CreateStoreAsync(newStore);
        return RedirectToAction("StoreList");
    }
    
    [HttpPost]
    public async Task<IActionResult> StoreEdit(StoreViewModel store)
    {
        var storeToUpdate = new Store
        {
            StoreId = store.StoreId,
            Name = store.Name,
            Address = store.Address,
            City = store.City,
            Phone = store.Phone,
            OpeningHours = store.OpeningHours
        };
        
        await _storeService.UpdateStoreAsync(storeToUpdate);
        return RedirectToAction("StoreList");
        
        
    }

}