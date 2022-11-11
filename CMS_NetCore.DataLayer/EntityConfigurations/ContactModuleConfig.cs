using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class ContactModuleConfig : IEntityTypeConfiguration<ContactModule>
{
    public void Configure(EntityTypeBuilder<ContactModule> builder)
    {
        builder.HasOne(contactModule => contactModule.Module)
            .WithOne();
    }
}