using System.Net;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaRent.API.Contracts;
using VillaRent.API.Contracts.DTO;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;

namespace VillaRent.API.Controllers;

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
    
    [Authorize(Roles = "admin")]
    [HttpGet(Name = "GetAllVillaNumbers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    [Authorize(Roles = "admin")]
    [HttpGet("{number:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int number)
    {
        try
        {
            if (number <= 0)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa number cannot be less than 0"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        
            var villaNumber = await villaNumberRepository.GetAsync( o => o.VillaNo == number, includeProperties:"Villa");
            if (villaNumber is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa number not found"];
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

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

    [Authorize(Roles = "admin")]
    [HttpPost("{number:int}", Name = "CreateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber(int number,
        [FromBody]VillaNumberCreateDto createDto)
    {
        try
        {
            if (number != createDto.VillaNo || number <= 0 )
            {
                _response.IsSuccess = false;
                _response.Errors = ["VillaNumber must contain valid number"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await villaNumberRepository.GetAsync(o => o.VillaNo == number) is not null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["VillaNumber already exists"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await villaRepository.GetAsync(v => v.Id == createDto.VillaId) is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["VillaId is invalid"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
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

    [Authorize(Roles = "admin")]
    [HttpDelete("{number:int}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int number)
    {
        try
        {
            if (number <= 0)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Incorrect villa number"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number);
            if (villaNumber is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa number to delete not found"];
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

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
    
    [Authorize(Roles = "admin")]
    [HttpPut("{number:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int number,
        [FromBody]VillaNumberUpdateDto updateDto)
    {
        try
        {
            if (number <= 0 || number != updateDto.VillaNo)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Incorrect villa number"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        
            if (await villaRepository.GetAsync(v => v.Id == updateDto.VillaId) is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["VillaId is invalid"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number, false);
            if (villaNumber is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa number to update not found"];
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

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

    [Authorize(Roles = "admin")]
    [HttpPatch("{number:int}", Name = "UpdatePartialVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePartialVillaNumber(int number,
        JsonPatchDocument<VillaNumberUpdateDto>? patchDto)
    {
        if (number <= 0 || patchDto is null) return BadRequest();

        var villaNumber = await villaNumberRepository.GetAsync(o => o.VillaNo == number, false);
        if (villaNumber is null) return NotFound();
        
        var villaNumberUpdateDto = mapper.Map<VillaNumberUpdateDto>(villaNumber);
        
        patchDto.ApplyTo(villaNumberUpdateDto, ModelState);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedVillaNumber = mapper.Map<VillaNumber>(villaNumberUpdateDto);
        await villaNumberRepository.UpdateAsync(updatedVillaNumber);

        return NoContent();
    }
}