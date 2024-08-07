using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using VillaRent.Persistence.Data;

namespace VillaRent.API.CustomMiddleware;

public static class CustomMiddleware
{
    public static async void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var migrator = dbContext.GetService<IMigrator>();
        await migrator.MigrateAsync();
    }
}