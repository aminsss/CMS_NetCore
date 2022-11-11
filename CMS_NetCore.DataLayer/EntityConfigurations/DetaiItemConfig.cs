using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class DetailItemConfig : IEntityTypeConfiguration<DetailItem>
{
    public void Configure(EntityTypeBuilder<DetailItem> modelBuilder)
    {
        modelBuilder.Property(detailItem => detailItem.DetailTitle).HasMaxLength(100);
        modelBuilder.Property(detailItem => detailItem.DetailType).HasMaxLength(100);

        modelBuilder.HasOne(detailItem => detailItem.DetailGroup)
            .WithMany(detailGroup => detailGroup.DetailItems)
            .HasForeignKey(detailItem => detailItem.DetailGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}