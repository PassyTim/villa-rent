using VillaRent.Web.Models.DTO;

namespace VillaRent.Web.Services.IServices;

public interface IVillaNumberService
{
    Task<T> GetAllAsync<T>(string? token);
    Task<T> GetAsync<T>(int id, string? token);
    Task<T> CreateAsync<T>(VillaNumberCreateDto createDto, string? token);
    Task<T> DeleteAsync<T>(int id, string? token);
    Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto, string? token);
}