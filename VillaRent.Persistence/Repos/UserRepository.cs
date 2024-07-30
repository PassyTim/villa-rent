using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repos;

public class UserRepository(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager)
    : IUserRepository
{
    
    // public async Task Add(ApplicationUser user, string password)
    // {
    //     await userManager.CreateAsync(user, password);
    //     await userManager.AddToRoleAsync(user, "admin");
    // }

    public async Task<ApplicationUser?> GetByUserName(string userName)
    {
        var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync
            (u => u.UserName!.ToLower() == userName.ToLower());

        return user;
    }
}