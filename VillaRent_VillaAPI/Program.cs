using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using VillaRent_VillaAPI.Configurations;
using VillaRent_VillaAPI.Data;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Repository;
using VillaRent_VillaAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("Logs/villaLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Host.UseSerilog();

builder.Services.AddResponseCaching();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default60", new CacheProfile
    {
        Duration = 60,
    });
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = 
            "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
            "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
            "Example \"Bearer 123sdhlf\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "VillaRent",
        Description = "Api to manage villa booking"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "2.0",
        Title = "VillaRent",
        Description = "Api to manage villa booking"
    });
});

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