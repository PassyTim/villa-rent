using VillaRent_Web.Models.DTO;

namespace VillaRent_Web.Services.IServices;

public interface IVillaNumberService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> CreateAsync<T>(VillaNumberCreateDto createDto);
    Task<T> DeleteAsync<T>(int id);
    Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto);
}