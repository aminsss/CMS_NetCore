
using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class AttributItemConfig : IEntityTypeConfiguration<AttributItem>
    {
        public void Configure(EntityTypeBuilder<AttributItem> modelBuilder)
        {
            //Property
            modelBuilder.Property(t => t.Name).HasMaxLength(100);
            modelBuilder.Property(t => t.value).HasMaxLength(100);
            modelBuilder.Property(t => t.idfilter).HasMaxLength(100);


            //Relations
            modelBuilder.HasOne(t => t.AttributGrp)
            .WithMany(r => r.AttributItem)
            .HasForeignKey(f => f.AttributGrpId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
