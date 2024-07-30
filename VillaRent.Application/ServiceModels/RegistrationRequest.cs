namespace VillaRent.Application.ServiceModels;

public record RegistrationRequest(
    string Username,  
    string Name ,
    string Password,
    bool IsAdmin = false);