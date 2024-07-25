namespace VillaRent_Web.Models.DTO;

public record LoginResponseDto(
    UserDto? User,
    string Token);