using VillaRent.Web.Models.DTO;

namespace VillaRent.Web.Services.IServices;

public interface IAuthService
{
    Task<T> LoginAsync<T>(LoginRequestDto loginObject);
    Task<T> RegisterAsync<T>(RegistrationRequestDto registrationObject);
}