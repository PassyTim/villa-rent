using System.ComponentModel.DataAnnotations;

namespace VillaRent.Web.Models.DTO;

public record VillaNumberUpdateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);