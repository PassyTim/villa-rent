namespace VillaRent.Web.Models.DTO;

public record RegistrationRequestDto(
    string Username,  
    string Name ,
    string Password, 
    string Role);