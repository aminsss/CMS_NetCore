using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class AttributeItemConfig : IEntityTypeConfiguration<AttributeItem>
{
    public void Configure(EntityTypeBuilder<AttributeItem> modelBuilder)
    {
        modelBuilder.Property(attributeItem => attributeItem.Name).HasMaxLength(100);
        modelBuilder.Property(attributeItem => attributeItem.Value).HasMaxLength(100);
        modelBuilder.Property(attributeItem => attributeItem.FilterId).HasMaxLength(100);

        modelBuilder.HasOne(attributeItem => attributeItem.AttributeGroup)
            .WithMany(attributeGroup => attributeGroup.AttributeItems)
            .HasForeignKey(attributeItem => attributeItem.AttributeGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}