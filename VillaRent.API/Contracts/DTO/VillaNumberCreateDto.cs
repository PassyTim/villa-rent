using System.ComponentModel.DataAnnotations;

namespace VillaRent.API.Contracts.DTO;

public record VillaNumberCreateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);