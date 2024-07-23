using System.ComponentModel.DataAnnotations;
using VillaRent_VillaAPI.Models;

namespace VillaRent_Web.Models.DTO;

public record VillaNumberCreateDto(
    [Required]
    int VillaNo,
    [Required]
    int VillaId,
    string? Details);