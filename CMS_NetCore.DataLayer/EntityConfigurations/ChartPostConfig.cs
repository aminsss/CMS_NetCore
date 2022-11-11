using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class ChartPostConfig : IEntityTypeConfiguration<ChartPost>
{
    public void Configure(EntityTypeBuilder<ChartPost> modelBuilder)
    {
        modelBuilder.Property(chartPost => chartPost.PostDuty).HasMaxLength(50);
    }
}