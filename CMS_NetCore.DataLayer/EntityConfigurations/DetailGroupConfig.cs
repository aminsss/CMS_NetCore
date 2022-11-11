using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class DetailGroupConfig : IEntityTypeConfiguration<DetailGroup>
{
    public void Configure(EntityTypeBuilder<DetailGroup> modelBuilder)
    {
        modelBuilder.Property(detailGroup => detailGroup.Name).HasMaxLength(50);

        modelBuilder.HasOne(detailGroup => detailGroup.ProductGroup)
            .WithMany(productGroup => productGroup.DetailGroups)
            .HasForeignKey(detailGroup => detailGroup.ProductGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}