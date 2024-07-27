using System.ComponentModel.DataAnnotations;

namespace VillaRent_VillaAPI.Models.DTO;

public record VillaNumberDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details,
    VillaDto Villa);