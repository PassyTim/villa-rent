using System.ComponentModel.DataAnnotations;

namespace VillaRent_VillaAPI.Models.DTO;

public record VillaDto(
    int Id,
    [Required] [MaxLength(50)] string Name,
    string? Details,
    [Required] double Rate,
    string? ImageUrl,
    string? Amenity,
    int Occupancy,
    int Sqft
);