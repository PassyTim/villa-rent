using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent_Web.Models;
using VillaRent_Web.Models.DTO;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;

    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper)
    {
        _villaNumberService = villaNumberService;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDto> list = new();
        var response = await _villaNumberService.GetAllAsync<APIResponse?>();
        
        if ( response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
    }

    public IActionResult CreateVillaNumber()
    {
        throw new NotImplementedException();
    }

    public IActionResult UpdateVillaNumber()
    {
        throw new NotImplementedException();
    }

    public IActionResult DeleteVillaNumber()
    {
        throw new NotImplementedException();
    }
}