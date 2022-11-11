using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class MultiPictureModuleConfig : IEntityTypeConfiguration<MultiPictureModule>
{
    public void Configure(EntityTypeBuilder<MultiPictureModule> builder)
    {
        builder.HasMany(multiPictureModule => multiPictureModule.MultiPictureItems)
            .WithOne(multiPictureItems => multiPictureItems.MultiPictureModule)
            .HasForeignKey(multiPictureItems => multiPictureItems.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}