using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repositories;

public class UserRepository(
    ApplicationDbContext dbContext)
    : IUserRepository
{
    
    public async Task<ApplicationUser?> GetByUserName(string userName)
    {
        var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync
            (u => u.UserName!.ToLower() == userName.ToLower());

        return user;
    }
}