using System.ComponentModel.DataAnnotations;

namespace VillaRent.API.Contracts.DTO;

public record VillaNumberUpdateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);