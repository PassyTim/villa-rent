using System.ComponentModel.DataAnnotations;

namespace VillaRent_Web.Models.DTO;

public record VillaNumberCreateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);