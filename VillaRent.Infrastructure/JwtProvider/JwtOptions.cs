namespace VillaRent.Infrastructure.JwtProvider;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresDays { get; set; }
}