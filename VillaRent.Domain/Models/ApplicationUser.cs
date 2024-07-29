using Microsoft.AspNetCore.Identity;

namespace VillaRent.Domain.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}