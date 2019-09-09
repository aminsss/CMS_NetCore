using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> modelBuilder)
        {
            //Property
            modelBuilder.Property(t => t.CityName).HasMaxLength(30);

            //Relations
            modelBuilder.HasOne(t => t.State)
            .WithMany(r => r.City)
            .HasForeignKey(r => new { r.StateId })
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
