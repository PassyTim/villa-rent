namespace VillaRent_Utility;

public static class StaticDetails
{
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH
    }

    public static readonly string SessionToken = "JWTToken";
}