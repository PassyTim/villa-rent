using VillaRent.Domain.Models;

namespace VillaRent.Domain.IRepositories;

public interface IUserRepository
{
    //Task Add(ApplicationUser user, string password);
    Task<ApplicationUser?> GetByUserName(string userName);
}