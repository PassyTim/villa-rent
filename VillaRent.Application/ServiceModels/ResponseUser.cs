namespace VillaRent.Application.ServiceModels;

public record ResponseUser(
    string ID,
    string? UserName,
    string? Name);