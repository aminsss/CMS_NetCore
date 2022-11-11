using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class ComponentConfig : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> modelBuilder)
    {
        modelBuilder.Property(component => component.ModuleName).HasMaxLength(30);
        modelBuilder.Property(component => component.AdminAction).HasMaxLength(30);
        modelBuilder.Property(component => component.AdminController).HasMaxLength(30);
        modelBuilder.Property(component => component.Name).HasMaxLength(30);
        modelBuilder.Property(component => component.ModuleType).HasMaxLength(30);
        modelBuilder.Property(component => component.Description).HasMaxLength(500);
    }
}