using VillaRent_Web.Models.DTO;

namespace VillaRent_Web.Services.IServices;

public interface IVillaService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> CreateAsync<T>(VillaCreateDto createDto);
    Task<T> DeleteAsync<T>(int id);
    Task<T> UpdateAsync<T>(VillaUpdateDto updateDto);
}