using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class StoreInfoConfig : IEntityTypeConfiguration<StoreInfo>
{
    public void Configure(EntityTypeBuilder<StoreInfo> modelBuilder)
    {
        modelBuilder.Property(storeInfo => storeInfo.ZindexMap).HasMaxLength(50);
        modelBuilder.Property(storeInfo => storeInfo.Banner).HasMaxLength(50);
    }
}