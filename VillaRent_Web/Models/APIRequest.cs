using static VillaRent_Utility.StaticDetails;

namespace VillaRent_Web.Models;

public class APIRequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; } = null!;
    public object? Data { get; init; }
    public string? Token { get; set; }
}