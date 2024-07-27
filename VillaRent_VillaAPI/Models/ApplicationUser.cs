using Microsoft.AspNetCore.Identity;

namespace VillaRent_VillaAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}