using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Models.DTO;
using VillaRent_VillaAPI.Repository.IRepository;

namespace VillaRent_VillaAPI.Controllers;

[Route("api/v{version:apiVersion}/usersAuth")]
[ApiController]
[ApiVersionNeutral]
public class UsersController(
    IUserRepository repository) : Controller
{
    private readonly APIResponse _apiResponse = new();

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var loginResponse = await repository.LoginUser(loginRequestDto);
        if (loginResponse.User is null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.Errors = ["Username or password is incorrect!"];
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_apiResponse);
        }

        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.Result = loginResponse;
        return Ok(_apiResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegistrationRequestDto registrationRequestDto)
    {
        bool isUserUnique = repository.IsUserUnique(registrationRequestDto.Username);
        if (!isUserUnique)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.Errors = ["This username is already used"];
            return BadRequest(_apiResponse);
        }

        var user = await repository.RegisterUser(registrationRequestDto);
        if (user is null)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.Errors = ["Error while registering"];
            return BadRequest(_apiResponse);
        }

        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }
}