using Microsoft.EntityFrameworkCore;
using VillaRent.Persistence.Data;

namespace VillaRent.API.CustomMiddleware;

public static class CustomMiddleware
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        dbContext.Database.Migrate();
    }
}