using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaRent.WebUtilities;
using VillaRent.Web.Models;
using VillaRent.Web.Models.DTO;
using VillaRent.Web.Services.IServices;

namespace VillaRent.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestData = new(Username: "", Password: "");
        return View(loginRequestData);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestData)
    {
        APIResponse? loginApiResponse = await _authService.LoginAsync<APIResponse?>(loginRequestData);
        if (loginApiResponse is not null && loginApiResponse.IsSuccess)
        {
            LoginResponseDto loginModel =
                JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(loginApiResponse.Result)!)!;

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.ReadJwtToken(loginModel.Token);
            
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(new Claim?[]
            {
                new(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name")!.Value),
                new(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role")!.Value)
            });

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            HttpContext.Session.SetString(StaticDetails.SessionRole, jwt.Claims.FirstOrDefault(u => u.Type == "role")!.Value);
            HttpContext.Session.SetString(StaticDetails.SessionToken, loginModel.Token);
            
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError("Errors", loginApiResponse.Errors.FirstOrDefault());
        return View(loginRequestData);
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestData)
    {
        APIResponse? registerResponse = await _authService.RegisterAsync<APIResponse?>(registrationRequestData);
        if (registerResponse is not null && registerResponse.IsSuccess)
            return RedirectToAction(nameof(Login));

        return View();
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(StaticDetails.SessionToken, "");
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}