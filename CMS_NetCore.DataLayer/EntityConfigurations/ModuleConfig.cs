using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class ModuleConfig : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasOne(module => module.HtmlModule)
            .WithOne(htmlModule => htmlModule.Module)
            .HasForeignKey<HtmlModule>(htmlModule => htmlModule.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(module => module.MenuModule)
            .WithOne(menuModule => menuModule.Module)
            .HasForeignKey<MenuModule>(menuModule => menuModule.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(module => module.ContactModule)
            .WithOne(contactModule => contactModule.Module)
            .HasForeignKey<ContactModule>(contactModule => contactModule.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(module => module.MultiPictureModule)
            .WithOne(multiPictureModule => multiPictureModule.Module)
            .HasForeignKey<MultiPictureModule>(multiPictureModule => multiPictureModule.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}