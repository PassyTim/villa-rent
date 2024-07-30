namespace VillaRent.Application.ServiceModels;

public record LoginResponse(
    ResponseUser? User,
    string Token);