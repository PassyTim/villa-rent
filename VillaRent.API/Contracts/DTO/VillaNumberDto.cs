using System.ComponentModel.DataAnnotations;
using VillaRent.API.Contracts.DTO;

namespace VillaRent.API.Contracts.DTO;

public record VillaNumberDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details,
    VillaDto Villa);