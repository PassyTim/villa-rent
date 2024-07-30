using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VillaRent.Application.IServices;
using VillaRent.Application.ServiceModels;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Infrastructure.JwtProvider.Interfaces;

namespace VillaRent.Application;

public class UserService(
    IUserRepository repository,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IJwtProvider jwtProvider,
    IMapper mapper) : IUserService
{
    public bool IsUserUnique(string username)
    {
        var user = repository.GetByUserName(username).Result;
        return user is null;
    }

    public async Task<LoginResponse> LoginUser(LoginUserRequest loginRequest)
    {
        var user = repository.GetByUserName(loginRequest.Username).Result;
        if(user is null) return new LoginResponse(User:null, Token:"");
        
        bool isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!isPasswordValid) return new LoginResponse(User:null, Token:"");
        
        var roles = await userManager.GetRolesAsync(user);
        string token = jwtProvider.Generate(user, roles);

        LoginResponse responseDto = new LoginResponse(
            User: mapper.Map<ResponseUser>(user), 
            Token: token);

        return responseDto;
    }

    public async Task<bool> TryRegisterUser(RegistrationRequest registrationRequest)
    {
        ApplicationUser userToCreate = new()
        {
            UserName = registrationRequest.Username,
            Email = registrationRequest.Username,
            NormalizedEmail = registrationRequest.Username.ToUpper(),
            Name = registrationRequest.Name
        };

        try
        {
            var createResult = await userManager.CreateAsync(userToCreate, registrationRequest.Password);
            if (createResult.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                    await roleManager.CreateAsync(new IdentityRole("customer"));
                }

                if (registrationRequest.IsAdmin)
                {
                    await userManager.AddToRoleAsync(userToCreate, "admin");
                }
                
                await userManager.AddToRoleAsync(userToCreate, "customer");

                var userToReturn = repository.GetByUserName(registrationRequest.Username).Result;
                return userToReturn is not null;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return false;
    }
}