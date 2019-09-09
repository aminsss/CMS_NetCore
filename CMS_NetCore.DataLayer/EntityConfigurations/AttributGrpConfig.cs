using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class AttributGrpConfig : IEntityTypeConfiguration<AttributGrp>
    {
        public void Configure(EntityTypeBuilder<AttributGrp> modelBuilder)
        {
            //Property
            modelBuilder.Property(t => t.Name).HasMaxLength(100);
            modelBuilder.Property(t => t.Attr_type).HasMaxLength(100);


            //Relations
            modelBuilder.HasOne(t => t.ProductGroup)
            .WithMany(r => r.AttributGrp)
            .HasForeignKey(f => f.ProductGroupId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
