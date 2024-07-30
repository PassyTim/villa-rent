using System.Net;

namespace VillaRent.Web.Models;

public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string>? Errors { get; set; } = null;
    public object? Result { get; set; }
}