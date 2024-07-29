using System.ComponentModel.DataAnnotations;

namespace VillaRent_VillaAPI.Models.DTO;

public record LoginRequestDto(
    [Required] string Username,
    [Required] string Password);