using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class DetailItemConfig : IEntityTypeConfiguration<DetailItem>
    {
        public void Configure(EntityTypeBuilder<DetailItem> modelBuilder)
        {
            //property
            modelBuilder.Property(t => t.DetailTitle).HasMaxLength(100);
            modelBuilder.Property(t => t.DetailType).HasMaxLength(100);


            //Relations
            modelBuilder.HasOne(t => t.DetailGroup)
                .WithMany(t => t.DetailItem)
                .HasForeignKey(t => t.DetailGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
