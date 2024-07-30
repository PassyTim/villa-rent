using System.ComponentModel.DataAnnotations;

namespace VillaRent.Web.Models.DTO;

public record VillaNumberDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details,
    VillaDto Villa);