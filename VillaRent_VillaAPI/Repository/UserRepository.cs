using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VillaRent_VillaAPI.Data;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Models.DTO;
using VillaRent_VillaAPI.Repository.IRepository;

namespace VillaRent_VillaAPI.Repository;

public class UserRepository(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
    : IUserRepository
{
    private string _secretKey = configuration.GetValue<string>("ApiSettings:Secret")!;
    
    public bool IsUserUnique(string username)
    {
        var user = dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
        if (user is null) return true;
        
        return false;
    }

    public async Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto)
    {
        var user = dbContext.ApplicationUsers.FirstOrDefault
        (u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        
        if (user is null || !isPasswordValid) return new LoginResponseDto(User:null, Token:"");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var roles = await userManager.GetRolesAsync(user);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponseDto responseDto = new LoginResponseDto(
            User: mapper.Map<UserDto>(user), 
            Token: tokenHandler.WriteToken(token)
            );

        return responseDto;
    }

    public async Task<UserDto> RegisterUser(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser userToCreate = new()
        {
            UserName = registrationRequestDto.Username,
            Email = registrationRequestDto.Username,
            NormalizedEmail = registrationRequestDto.Username.ToUpper(),
            Name = registrationRequestDto.Name
        };

        try
        {
            var createResult = await userManager.CreateAsync(userToCreate, registrationRequestDto.Password);
            if (createResult.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                    await roleManager.CreateAsync(new IdentityRole("customer"));
                }
                await userManager.AddToRoleAsync(userToCreate, "admin");
                var userToReturn = dbContext.ApplicationUsers
                    .FirstOrDefault(u => u.UserName == registrationRequestDto.Username);
                
                return mapper.Map<UserDto>(userToReturn);
                // добавить возврат ошибок на фронтенд
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return new UserDto(ID:null, UserName:null, Name:null);
    }
}