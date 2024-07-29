using VillaRent.Domain.Models;

namespace VillaRent.Infrastructure.JwtProvider.Interfaces;

public interface IJwtProvider
{
    string Generate(ApplicationUser user, IList<string> roles);
}