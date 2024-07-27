namespace VillaRent_VillaAPI.Models.DTO;

public record LoginResponseDto(
    UserDto? User,
    string Token);