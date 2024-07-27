using System.ComponentModel.DataAnnotations;

namespace VillaRent_VillaAPI.Models.DTO;

public record VillaNumberUpdateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);