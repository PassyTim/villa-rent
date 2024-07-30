using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent.WebUtilities;
using VillaRent.Web.Models;
using VillaRent.Web.Models.DTO;
using VillaRent.Web.Services.IServices;

namespace VillaRent.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVillaService _villaService;

    public HomeController(IVillaService villaService, ILogger<HomeController> logger)
    {
        _villaService = villaService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        List<VillaDto> list = new(); 
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        var response = await _villaService.GetAllAsync<APIResponse?>(token);

        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}