using System.Net;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent.API.Contracts.DTO;
using VillaRent.API.Contracts;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;

namespace VillaRent.API.Controllers;

[Route("api/v{version:apiVersion}/villaAPI")]
[ApiController]
[ApiVersion("1.0")]
public class VillaApiController(
    ILogger<VillaApiController> logger,
    IVillaRepository repository,
    IMapper mapper)
    : ControllerBase
{
    private readonly APIResponse _response = new();
    
    [HttpGet]
    [ResponseCache(CacheProfileName = "Default60")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> GetVillas(int pageSize = 0, int pageNumber = 1)    
    {
        try
        {
            logger.LogInformation("GetVillas called.");
            
            Pagination pagination = new Pagination { PageNumber = pageNumber, PageSize = pageSize };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagination));
            
            _response.Result = mapper.Map<List<VillaDto>>(await repository.
                GetAllAsync(pageSize:pageSize, pageNumber:pageNumber));
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
    
    [Authorize]
    [ResponseCache(CacheProfileName = "Default60")]
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                logger.LogError("Get villa with Id {Id}", id);
                _response.Errors = ["Id cannot be 0"];
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var villa = await repository.GetAsync(v => v.Id == id);
            if (villa is not null)
            {
                _response.Result = mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }

            _response.Errors = ["Villa doesn't exist!"];
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsSuccess = false;
            return NotFound(_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }
        
        return _response;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDto? createDto)
    {
        try
        {
            if (await repository.GetAsync(v => 
                    v.Name.ToLower() == createDto!.Name.ToLower()) is not null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa with this name already exists"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (createDto is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Data is null"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);  
            }
            

            var model = mapper.Map<Villa>(createDto);
            await repository.CreateAsync(model);

            _response.Result = mapper.Map<VillaDto>(model);
            _response.StatusCode = HttpStatusCode.Created;
        
            return CreatedAtRoute("GetVilla", new {id = model.Id},_response);
        }
        catch (Exception ex)
        {
            _response.Errors = [ex.Message];
            _response.IsSuccess = false;
        }

        return _response;
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Id cannot be null"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);  
            }

            var villa = await repository.GetAsync(v => v.Id == id);
            if (villa is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa to delete not found"];
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await repository.RemoveAsync(villa);
            
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
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto? updateDto)
    {
        try
        {
            if (updateDto is null || id != updateDto.Id)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Incorrect update data"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaToUpdate = await repository.GetAsync(v => v.Id == id, false);
            if (villaToUpdate is null)
            {
                _response.IsSuccess = false;
                _response.Errors = ["Villa to update not found"];
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
        
            var model = mapper.Map<Villa>(updateDto);
            await repository.UpdateAsync(model);

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
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto>? patchDto)
    {
        if (patchDto is null || id == 0)
        {
            _response.IsSuccess = false;
            _response.Errors = ["Incorrect update data"];
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        var villaToUpdate = await repository.GetAsync(v => v.Id == id, false);
        if (villaToUpdate is null)
        {
            _response.IsSuccess = false;
            _response.Errors = ["Villa to update not found"];
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }
        
        var villaDto = mapper.Map<VillaUpdateDto>(villaToUpdate);
        
        patchDto.ApplyTo(villaDto, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = mapper.Map<Villa>(villaDto);

        await repository.UpdateAsync(model);
        
        return NoContent();
    }
}