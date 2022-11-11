using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> modelBuilder)
    {
        modelBuilder.Property(city => city.CityName).HasMaxLength(30);

        modelBuilder.HasOne(city => city.State)
            .WithMany(state => state.Cities)
            .HasForeignKey(city => new { city.StateId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}