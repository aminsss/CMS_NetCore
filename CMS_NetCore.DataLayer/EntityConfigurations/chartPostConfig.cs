using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class chartPostConfig : IEntityTypeConfiguration<chartPost>
    {
        public void Configure(EntityTypeBuilder<chartPost> modelBuilder)
        {
            modelBuilder.Property(t => t.Postduty).HasMaxLength(50);
        }
    }
}
