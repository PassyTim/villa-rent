using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VillaRent.API.Contracts;
using VillaRent.Application.IServices;
using VillaRent.Application.ServiceModels;

namespace VillaRent.API.Controllers;

[Route("api/v{version:apiVersion}/usersAuth")]
[ApiController]
[ApiVersionNeutral]
public class UsersController(
    IUserService userService) : Controller
{
    private readonly APIResponse _apiResponse = new();

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequest)
    {
        var loginResponse = await userService.LoginUser(loginRequest);
        // реализовать передачу токена в хедерах
        HttpContext.Response.Headers.Authorization = loginResponse.Token;
        
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
    public async Task<IActionResult> Register([FromBody]RegistrationRequest registrationRequest)
    {
        bool isUserUnique = userService.IsUserUnique(registrationRequest.Username);
        if (!isUserUnique)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.Errors = ["This username is already used"];
            return BadRequest(_apiResponse);
        }

        var isSuccess = await userService.TryRegisterUser(registrationRequest);
        if (!isSuccess)
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