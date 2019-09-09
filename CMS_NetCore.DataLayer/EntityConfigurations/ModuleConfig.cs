using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class ModuleConfig : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {

            //Relations
            builder.HasOne(t => t.HtmlModule)
                .WithOne(t => t.Module)
                .HasForeignKey<HtmlModule>(t=>t.HtmlModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.MenuModule)
                .WithOne(t => t.Module)
                .HasForeignKey<MenuModule>(t=>t.MenuModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.ContactModule)
                .WithOne(t => t.Module)
                .HasForeignKey<ContactModule>(t=>t.ContactModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
