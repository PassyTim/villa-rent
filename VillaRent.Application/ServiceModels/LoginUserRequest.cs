using System.ComponentModel.DataAnnotations;

namespace VillaRent.Application.ServiceModels;

public record LoginUserRequest(
    [Required] string Username,
    [Required] string Password);