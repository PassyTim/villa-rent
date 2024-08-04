using VillaRent.WebUtilities;
using VillaRent.Web.Models;
using VillaRent.Web.Models.DTO;
using VillaRent.Web.Services.IServices;

namespace VillaRent.Web.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly string _villaUrl;
    public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) 
        : base(httpClient)
    {
        _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi")!;
    }

    public Task<T> LoginAsync<T>(LoginRequestDto loginObject)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = loginObject,
            Url = _villaUrl + "/api/usersAuth/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegistrationRequestDto registrationObject)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = registrationObject,
            Url = _villaUrl + "/api/usersAuth/register"
        });
    }
}