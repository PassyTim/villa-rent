using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using VillaRent_Utility;
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
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var response = await _villaNumberService.GetAllAsync<APIResponse?>(token);
        
        if ( response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result)!)!;
        }
        
        return View(list);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateViewModel viewModel = new();
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var response = await _villaService.GetAllAsync<APIResponse?>(token);
        
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

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateViewModel viewModel)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        if (ModelState.IsValid)
        {
            APIResponse? response = await _villaNumberService.CreateAsync<APIResponse?>(viewModel.VillaNumber, token);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "VillaNumber created successfully!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            TempData["error"] = "An error occured!";
            if (response.Errors.Count > 0)
                ModelState.TryAddModelError("Errors", response.Errors.FirstOrDefault()!);
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>(token);
        
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

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        var firstResponse = await _villaNumberService.GetAsync<APIResponse?>(villaNo, token);
        VillaNumberUpdateViewModel viewModel = new();
        
        if (firstResponse is not null && firstResponse.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(firstResponse.Result)!)!;

            viewModel.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);
        }
        
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>(token);
        
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
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateViewModel updateViewModel)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        var response = await _villaNumberService.UpdateAsync<APIResponse?>(updateViewModel.VillaNumber, token);
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "VillaNumber updated successfully!";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>(token);
        
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
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        var firstResponse = await _villaNumberService.GetAsync<APIResponse?>(villaNo, token);
        VillaNumberDeleteViewModel viewModel = new();
        
        if (firstResponse is not null && firstResponse.IsSuccess)
        {
            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(firstResponse.Result)!)!;
            viewModel.VillaNumber = model;
        }

        var secondResponse = await _villaService.GetAllAsync<APIResponse?>(token);
        
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

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteViewModel deleteViewModel)
    {
        string? token = HttpContext.Session.GetString(StaticDetails.SessionToken);
        
        var response = await _villaNumberService.DeleteAsync<APIResponse?>(deleteViewModel.VillaNumber.VillaNo, token);
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "VillaNumber deleted successfully!";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        
        TempData["error"] = "An error occured!";
        var secondResponse = await _villaService.GetAllAsync<APIResponse?>(token);
        
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