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
        var response = await _villaService.GetAllAsync<APIResponse?>();

        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
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
            var response = await _villaService.CreateAsync<APIResponse?>(createDto);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(createDto);
    }
    
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaService.GetAsync<APIResponse?>(villaId);
        if (response is not null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result)!)!;
            return View(_mapper.Map<VillaUpdateDto>(model));
        }
        
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDto updateDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.UpdateAsync<APIResponse?>(updateDto);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }

        return View(updateDto);
    }
    
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaService.GetAsync<APIResponse?>(villaId);
        if (response is not null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result)!)!;
            return View(model);
        }
        
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDto model)
    {
        var response = await _villaService.DeleteAsync<APIResponse?>(model.Id);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVilla));
        }

        return View(model);
    }
}