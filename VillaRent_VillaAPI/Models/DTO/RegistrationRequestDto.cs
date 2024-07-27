namespace VillaRent_VillaAPI.Models.DTO;

public record RegistrationRequestDto(
    string Username,  
    string Name ,
    string Password, 
    string Role);