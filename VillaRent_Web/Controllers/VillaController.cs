using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent_Utility;
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
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        var response = await _villaService.GetAllAsync<APIResponse?>(token);

        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDto createDto)
    {
        if (ModelState.IsValid)
        {
            string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            var response = await _villaService.CreateAsync<APIResponse?>(createDto, token);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Villa created successfully!";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "An error occured!";
        return View(createDto);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var response = await _villaService.GetAsync<APIResponse?>(villaId, token);
        if (response is not null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result)!)!;
            return View(_mapper.Map<VillaUpdateDto>(model));
        }
        
        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDto updateDto)
    {
        if (ModelState.IsValid)
        {
            string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            var response = await _villaService.UpdateAsync<APIResponse?>(updateDto, token);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Villa updated successfully!";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "An error occured!";
        return View(updateDto);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var response = await _villaService.GetAsync<APIResponse?>(villaId, token);
        if (response is not null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result)!)!;
            return View(model);
        }
        
        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDto model)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var response = await _villaService.DeleteAsync<APIResponse?>(model.Id, token);
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "Villa deleted successfully!";
            return RedirectToAction(nameof(IndexVilla));
        }

        TempData["error"] = "An error occured!";
        return View(model);
    }
}