using System.ComponentModel.DataAnnotations;

namespace VillaRent_VillaAPI.Models.DTO;

public record VillaNumberCreateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);