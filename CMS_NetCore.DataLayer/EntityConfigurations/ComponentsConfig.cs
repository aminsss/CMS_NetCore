using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class ComponentConfig : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> modelBuilder)
        {
            //Property
            modelBuilder.Property(t => t.ActionName).HasMaxLength(30);
            modelBuilder.Property(t => t.AdminAction).HasMaxLength(30);
            modelBuilder.Property(t => t.AdminController).HasMaxLength(30);
            modelBuilder.Property(t => t.ComponentName).HasMaxLength(30);
            modelBuilder.Property(t => t.ControllerName).HasMaxLength(30);
            modelBuilder.Property(t => t.Descroption).HasMaxLength(500);
        }
    }
}
