using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using VillaRent.API.Configurations;
using VillaRent.API.Extensions;
using VillaRent.Application;
using VillaRent.Application.IServices;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Infrastructure.JwtProvider;
using VillaRent.Infrastructure.JwtProvider.Interfaces;
using VillaRent.Persistence.Data;
using VillaRent.Persistence.Repositories;

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

services.AddScoped<VillaRepository>();
services.AddScoped<IVillaRepository>(provider =>
{
    var villaRepository = provider.GetService<VillaRepository>();
    return new CachedVillaRepository(villaRepository, provider.GetService<IDistributedCache>()!);
});

services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = configuration.GetConnectionString("Redis")!;
    redisOptions.Configuration = connection;
});

services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IJwtProvider, JwtProvider>();

services.AddConfiguredApiVersioning();

services.AddAutoMapper(typeof(MappingConfig));

services.AddControllers().AddNewtonsoftJson();

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