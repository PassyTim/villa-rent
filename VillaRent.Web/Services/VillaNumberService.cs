using VillaRent.WebUtilities;
using VillaRent.Web.Models;
using VillaRent.Web.Models.DTO;
using VillaRent.Web.Services.IServices;

namespace VillaRent.Web.Services;

public class VillaNumberService (
    IHttpClientFactory httpClient,
    IConfiguration configuration
    )
    : BaseService(httpClient), IVillaNumberService
{
    
    private readonly string _apiUrl = configuration.GetValue<string>("ServiceUrls:VillaApi")!;

    public Task<T> GetAllAsync<T>(string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = _apiUrl + "/api/v1/villaNumberAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = _apiUrl + "/api/v1/villaNumberAPI/" + id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDto createDto, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = createDto,
            Url = _apiUrl + "/api/v1/villaNumberAPI/" + createDto.VillaNo,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = _apiUrl + "/api/v1/villaNumberAPI/" + id,
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = updateDto,
            Url = _apiUrl + "/api/v1/villaNumberAPI/" + updateDto.VillaNo,
            Token = token
        });
    }
}