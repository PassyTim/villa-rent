using System.ComponentModel.DataAnnotations;

namespace VillaRent_Web.Models.DTO;

public record VillaUpdateDto(
    [Required] int Id,
    [Required] [MaxLength(50)] string Name,
    string Details,
    [Required] double Rate,
    [Required] string ImageUrl,
    string Amenity,
    [Required] int Occupancy,
    [Required] int Sqft
);
