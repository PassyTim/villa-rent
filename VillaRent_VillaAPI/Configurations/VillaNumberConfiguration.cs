using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaRent_VillaAPI.Models;

namespace VillaRent_VillaAPI.Configurations;

public class VillaNumberConfiguration : IEntityTypeConfiguration<VillaNumber>
{
    public void Configure(EntityTypeBuilder<VillaNumber> builder)
    {
        builder.HasOne(p => p.Villa)
            .WithMany();
    }
}