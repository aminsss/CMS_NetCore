using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class AttributeGroupConfig : IEntityTypeConfiguration<AttributeGroup>
{
    public void Configure(EntityTypeBuilder<AttributeGroup> modelBuilder)
    {
        modelBuilder.Property(attributeGroup => attributeGroup.Name).HasMaxLength(100);
        modelBuilder.Property(attributeGroup => attributeGroup.AttributeType).HasMaxLength(100);

        modelBuilder.HasOne(attributeGroup => attributeGroup.ProductGroup)
            .WithMany(productGroup => productGroup.AttributeGroups)
            .HasForeignKey(attributeGroup => attributeGroup.ProductGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}