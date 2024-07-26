using VillaRent_Utility;
using VillaRent_Web.Models;
using VillaRent_Web.Models.DTO;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Services;

public class VillaService(
    IHttpClientFactory httpClient,
    IConfiguration configuration) 
    : BaseService(httpClient), IVillaService
{
    private readonly string _url = configuration.GetValue<string>("ServiceUrls:VillaApi")!;
    
    public Task<T> GetAllAsync<T>(string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = _url+"/api/villaAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = _url + "/api/villaAPI/" + id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDto createDto, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = createDto,
            Url = _url+"/api/villaAPI",
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = _url + "/api/villaAPI/" + id,
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDto updateDto, string? token)
    {
        return SendAsync<T>(new APIRequest
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = updateDto,
            Url = _url+"/api/villaAPI/" + updateDto.Id,
            Token = token
        });
    }
}