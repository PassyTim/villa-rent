using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent_Web.Models;
using VillaRent_Web.Models.DTO;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDto> list = new(); 
        var response = await _villaService.GetAllAsync<APIResponse>();

        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
    }

    public IActionResult UpdateVilla()
    {
        throw new NotImplementedException();
    }

    public IActionResult DeleteVilla()
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDto createDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.CreateAsync<APIResponse>(createDto);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(createDto);
    }
}