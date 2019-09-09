using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class ContactModuleConfig : IEntityTypeConfiguration<ContactModule>
    {
        public void Configure(EntityTypeBuilder<ContactModule> builder)
        {
            builder.HasOne(t => t.Module)
                 .WithOne();
        }
    }
}
