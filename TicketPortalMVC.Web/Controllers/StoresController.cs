using Microsoft.AspNetCore.Mvc;
using TicketPortalMVC.Application.Services.Interface;

namespace TicketPortalMVC.Web.Controllers;

public class StoresController : Controller
{
    private readonly IStoreService _storeService;
    
    public StoresController(IStoreService storeService)
    {
        _storeService = storeService;
    }
    
    [HttpGet("/nase-prodejny")]
    public IActionResult Index()
    {
        var stores = _storeService.GetStoresAsync().Result;
        return View(stores);
    }
}