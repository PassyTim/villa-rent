using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VillaRent_VillaAPI.Configurations;
using VillaRent_VillaAPI.Models;

namespace VillaRent_VillaAPI.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("DefaultSQLConnection"));
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new VillaConfiguration());
    }
}