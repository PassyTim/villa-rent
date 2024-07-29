namespace VillaRent.Domain.IRepositories;

public interface IUserRepository
{
    bool IsUserUnique(string username);
    Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);
    Task<UserDto> RegisterUser(RegistrationRequestDto registrationRequestDto);
}