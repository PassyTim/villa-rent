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
        VillaNumberCreateViewModel viewModel = new();
        var response = await _villaService.GetAllAsync<APIResponse?>();
        
        if (response is not null && response.IsSuccess)
        {
            viewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(response.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<APIResponse?>(viewModel.VillaNumber);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "VillaNumber created successfully!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                TempData["error"] = "An error occured!";
                if (response.Errors.Count > 0)
                    ModelState.TryAddModelError("Errors", response.Errors.FirstOrDefault()!);
            }
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>();
        
        if (secondResponse is not null && secondResponse.IsSuccess)
        {
            viewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(secondResponse.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        return View(viewModel);
    }

    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        var firstResponse = await _villaNumberService.GetAsync<APIResponse?>(villaNo);
        VillaNumberUpdateViewModel viewModel = new();
        
        if (firstResponse is not null && firstResponse.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(firstResponse.Result)!)!;

            viewModel.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);
        }
        
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>();
        
        if (secondResponse is not null && secondResponse.IsSuccess)
        {
            viewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(secondResponse.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            return View(viewModel);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateViewModel updateViewModel)
    {
        var response = await _villaNumberService.UpdateAsync<APIResponse?>(updateViewModel.VillaNumber);
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "VillaNumber updated successfully!";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>();
        
        if (secondResponse is not null && secondResponse.IsSuccess)
        {
            updateViewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(secondResponse.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        return View(updateViewModel);
    }
    
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        var firstResponse = await _villaNumberService.GetAsync<APIResponse?>(villaNo);
        VillaNumberDeleteViewModel viewModel = new();
        
        if (firstResponse is not null && firstResponse.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(firstResponse.Result)!)!;
            viewModel.VillaNumber = model;
        }

        var secondResponse = await _villaService.GetAllAsync<APIResponse?>();
        
        if (secondResponse is not null && secondResponse.IsSuccess)
        {
            viewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(secondResponse.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            return View(viewModel);
        }
        
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteViewModel deleteViewModel)
    {
        var response = await _villaNumberService.DeleteAsync<APIResponse?>(deleteViewModel.VillaNumber.VillaNo);
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "VillaNumber deleted successfully!";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>();
        
        if (secondResponse is not null && secondResponse.IsSuccess)
        {
            deleteViewModel.Villas = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(secondResponse.Result)!)!.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        
        return View(deleteViewModel);
    }
}