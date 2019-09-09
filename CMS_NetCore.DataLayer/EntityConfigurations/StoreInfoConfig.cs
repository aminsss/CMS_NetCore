using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations 
{
    public class StoreInfoConfig : IEntityTypeConfiguration<StoreInfo>
    {
        public void Configure(EntityTypeBuilder<StoreInfo> modelBuilder)
        {
            //property
            modelBuilder.Property(t => t.ZindexMap).HasMaxLength(50);
            modelBuilder.Property(t => t.banner).HasMaxLength(50);
        }
    }
}
