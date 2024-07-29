using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VillaRent_VillaAPI.Configurations;
using VillaRent_VillaAPI.Data;
using VillaRent_VillaAPI.Extensions;
using VillaRent_VillaAPI.JwtProvider;
using VillaRent_VillaAPI.JwtProvider.Interfaces;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Repository;
using VillaRent_VillaAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("Logs/villaLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

services.AddResponseCaching();

services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddDbContext<ApplicationDbContext>();

services.AddScoped<IVillaRepository, VillaRepository>();
services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IJwtProvider, JwtProvider>();

services.AddConfiguredApiVersioning();

services.AddAutoMapper(typeof(MappingConfig));

services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default60", new CacheProfile
    {
        Duration = 60,
    });
}).AddNewtonsoftJson();

services.AddEndpointsApiExplorer();
services.AddSwagger();

services.AddApiAuthentication(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "VillaRentV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "VillaRentV2");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();