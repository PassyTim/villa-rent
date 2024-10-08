using System.ComponentModel.DataAnnotations;

namespace VillaRent.Web.Models.DTO;

public record VillaCreateDto(
    [Required] [MaxLength(50)] string Name,
    string? Details,
    [Required] double Rate,
    string? ImageUrl,
    string? Amenity,
    int Occupancy,
    int Sqft
    );