using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VillaRent_Utility;
using VillaRent_Web.Models;
using VillaRent_Web.Services.IServices;

namespace VillaRent_Web.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    private IHttpClientFactory HttpClient { get; }

    protected BaseService(IHttpClientFactory httpClient)
    {
        responseModel = new APIResponse();
        this.HttpClient = httpClient;
    }
    
    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = HttpClient.CreateClient("RentAPI");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.RequestUri = new Uri(apiRequest.Url);

            if (apiRequest.Data is not null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            requestMessage.Method = apiRequest.ApiType switch
            {
                StaticDetails.ApiType.POST => HttpMethod.Post,
                StaticDetails.ApiType.PUT => HttpMethod.Put,
                StaticDetails.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            if (!apiRequest.Token.IsNullOrEmpty())
            {
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }
            
            HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage);

            var apiResponseContentAsString = await httpResponseMessage.Content.ReadAsStringAsync();

            try
            {
                APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiResponseContentAsString)!;
                if (response is not null && 
                    httpResponseMessage.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
                {
                    response.IsSuccess = false;
                    var outgoingResponseAsString = JsonConvert.SerializeObject(response);
                    var outgoingResponse = JsonConvert.DeserializeObject<T>(outgoingResponseAsString);
                    return outgoingResponse;
                }
            }
            catch (Exception ex)
            {
                var exceptionOutgoingResponse = JsonConvert.DeserializeObject<T>(apiResponseContentAsString)!;
                return exceptionOutgoingResponse; 
            }
            
            var defaultOutgoingResponse = JsonConvert.DeserializeObject<T>(apiResponseContentAsString)!;
            return defaultOutgoingResponse;
        }
        catch (Exception ex)
        {
            var exceptionApiResponse = new APIResponse
            {
                Errors = [ex.ToString()],
                IsSuccess = false
            };
            var exceptionApiResponseAsString = JsonConvert.SerializeObject(exceptionApiResponse);
            var response = JsonConvert.DeserializeObject<T>(exceptionApiResponseAsString)!;
            return response;
        }
    }
}