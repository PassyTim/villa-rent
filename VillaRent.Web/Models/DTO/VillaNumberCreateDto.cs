using System.ComponentModel.DataAnnotations;

namespace VillaRent.Web.Models.DTO;

public record VillaNumberCreateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);