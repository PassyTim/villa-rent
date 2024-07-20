using System.Text;
using Newtonsoft.Json;
using VillaRent_Utility;
using VillaRent_Web.Models;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    private IHttpClientFactory httpClient { get; }

    protected BaseService(IHttpClientFactory httpClient)
    {
        responseModel = new APIResponse();
        this.httpClient = httpClient;
    }
    
    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("RentAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);

            if (apiRequest.Data is not null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            message.Method = apiRequest.ApiType switch
            {
                StaticDetails.ApiType.POST => HttpMethod.Post,
                StaticDetails.ApiType.PUT => HttpMethod.Put,
                StaticDetails.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(apiContent)!;
            return response; 
        }
        catch (Exception ex)
        {
            var dto = new APIResponse
            {
                Errors = [ex.ToString()],
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var response = JsonConvert.DeserializeObject<T>(res)!;
            return response;
        }
    }
}