using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Models.DTO;

namespace VillaRent_VillaAPI.Repository.IRepository;

public interface IUserRepository
{
    bool IsUserUnique(string username);
    Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);
    Task<UserDto> RegisterUser(RegistrationRequestDto registrationRequestDto);
}