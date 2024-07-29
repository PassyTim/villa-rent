using VillaRent_VillaAPI.Models;

namespace VillaRent_VillaAPI.JwtProvider.Interfaces;

public interface IJwtProvider
{
    string Generate(ApplicationUser user, IList<string> roles);
}