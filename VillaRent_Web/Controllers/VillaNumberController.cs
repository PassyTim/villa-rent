using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using VillaRent_Web.Models;
using VillaRent_Web.Models.DTO;
using VillaRent_Web.Models.ViewModels;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;

    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberService villaNumberService, 
        IVillaService villaService, IMapper mapper)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
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

    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM viewModel = new();
        var response = await _villaService.GetAllAsync<APIResponse?>();
        
        if (response is not null && response.IsSuccess)
        {
            viewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM viewModel)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<APIResponse?>(viewModel.VillaNumber);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }

        return View(viewModel);
    }

    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        var response = await _villaNumberService.GetAsync<APIResponse?>(villaNo);
        if (response is not null && response.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result)!)!;
            return View(_mapper.Map<VillaNumberUpdateDto>(model));
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDto updateDto)
    {
        var response = await _villaNumberService.UpdateAsync<APIResponse?>(updateDto);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }

        return View(updateDto);
    }
    
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        var response = await _villaNumberService.GetAsync<APIResponse?>(villaNo);
        if (response is not null && response.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result)!)!;
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDto model)
    {
        var response = await _villaNumberService.DeleteAsync<APIResponse?>(model.VillaNo);
        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }

        return View(model);
    }
}