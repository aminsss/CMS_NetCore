using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class DetailGroupsConfig : IEntityTypeConfiguration<DetailGroup>
    {
        public void Configure(EntityTypeBuilder<DetailGroup> modelBuilder)
        {
            //Property
            modelBuilder.Property(t => t.Name).HasMaxLength(50);

            //Relations
            modelBuilder.HasOne(t => t.ProductGroup)
            .WithMany(t => t.DetailGroups)
            .HasForeignKey(t => t.ProductGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
