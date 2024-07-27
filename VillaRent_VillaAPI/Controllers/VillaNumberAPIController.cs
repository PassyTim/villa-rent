using System.Net;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Models.DTO;
using VillaRent_VillaAPI.Repository.IRepository;

namespace VillaRent_VillaAPI.Controllers;

[Route("api/v{version:apiVersion}/villaNumberAPI")]
[ApiController]
[ApiVersion("1.0")]
public class VillaNumberAPIController(
    IVillaNumberRepository villaNumberRepository,
    IVillaRepository villaRepository,
    IMapper mapper) 
    : ControllerBase
{
    private readonly APIResponse _response = new();
    
    [HttpGet(Name = "GetAllVillaNumbers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            List<VillaNumber> villaNumbers = await villaNumberRepository.GetAllAsync(includeProperties:"Villa");
            var villaNumbersDto = mapper.Map<List<VillaNumberDto>>(villaNumbers);

            _response.Result = villaNumbersDto;
            _response.StatusCode = HttpStatusCode.OK;
            
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }
        return _response;
    }

    [HttpGet("{number:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int number)
    {
        try
        {
            if (number <= 0) return BadRequest();
        
            var villaNumber = await villaNumberRepository.GetAsync( o => o.VillaNo == number, includeProperties:"Villa");
            if (villaNumber is null) return NotFound();

            var villaNumberDto = mapper.Map<VillaNumberDto>(villaNumber);
            _response.Result = villaNumberDto;
            _response.StatusCode = HttpStatusCode.OK;
        
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }

        return _response;
    }

    [HttpPost("{number:int}", Name = "CreateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber(int number,
        [FromBody]VillaNumberCreateDto createDto)
    {
        try
        {
            if (number != createDto.VillaNo || number <= 0 )
            {
                ModelState.AddModelError("Errors", "VillaNumber must contain valid number");
                return BadRequest(ModelState);
            }

            if (await villaNumberRepository.GetAsync(o => o.VillaNo == number) is not null)
            {
                ModelState.AddModelError("Errors", "VillaNumber already exists");
                return BadRequest(ModelState);
            }

            if (await villaRepository.GetAsync(v => v.Id == createDto.VillaId) is null)
            {
                ModelState.AddModelError("Errors", "VillaId is invalid");
                return BadRequest(ModelState);
            }
        
            var villaNumber = mapper.Map<VillaNumber>(createDto);
            villaNumber.CreatedDate = DateTime.Now;
            await villaNumberRepository.CreateAsync(villaNumber);
        
            _response.Result = mapper.Map<VillaNumberDto>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;
        
            return CreatedAtRoute("GetVillaNumber", 
                new {number = villaNumber.VillaNo}, _response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }
        return _response;
    }

    [HttpDelete("{number:int}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int number)
    {
        try
        {
            if (number <= 0) return BadRequest();

            var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number);
            if (villaNumber is null) return NotFound();

            await villaNumberRepository.RemoveAsync(villaNumber);

            _response.StatusCode = HttpStatusCode.OK;
        
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }

        return _response;
    }
    
    [HttpPut("{number:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int number,
        [FromBody]VillaNumberUpdateDto updateDto)
    {
        try
        {
            if (number <= 0 || number != updateDto.VillaNo) return BadRequest();
        
            if (await villaRepository.GetAsync(v => v.Id == updateDto.VillaId) is null)
            {
                ModelState.AddModelError("Errors", "VillaId is invalid");
                return BadRequest(ModelState);
            }

            var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number, false);
            if (villaNumber is null) return NotFound();

            var updatedVillaNumber = mapper.Map<VillaNumber>(updateDto);
            await villaNumberRepository.UpdateAsync(updatedVillaNumber);
            
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }
        return _response;
    }

    [HttpPatch("{number:int}", Name = "UpdatePartialVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePartialVillaNumber(int number,
        JsonPatchDocument<VillaNumberUpdateDto> patchDto)
    {
        if (number <= 0 || patchDto is null) return BadRequest();

        var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number, false);
        if (villaNumber is null) return NotFound();

        // дописать валидацию чтобы айди и ссылочность совпадали
        var villaNumberUpdateDto = mapper.Map<VillaNumberUpdateDto>(villaNumber);
        
        patchDto.ApplyTo(villaNumberUpdateDto, ModelState);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedVillaNumber = mapper.Map<VillaNumber>(villaNumberUpdateDto);
        await villaNumberRepository.UpdateAsync(updatedVillaNumber);

        return NoContent();
    }
}