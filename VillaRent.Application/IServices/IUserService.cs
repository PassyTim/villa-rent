using VillaRent.Application.ServiceModels;

namespace VillaRent.Application.IServices;

public interface IUserService
{
    bool IsUserUnique(string userName);
    Task<bool> TryRegisterUser(RegistrationRequest registrationRequest);
    Task<LoginResponse> LoginUser(LoginUserRequest loginRequest);
}