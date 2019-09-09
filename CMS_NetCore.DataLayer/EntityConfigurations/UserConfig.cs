using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  CMS_NetCore.DataLayer.EntityConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Relations
            builder.HasMany(t => t.Messages)
                .WithOne(t => t.UsersFrom)
                .HasForeignKey(t => t.FromUser);


            //Relations
            builder.HasMany(t => t.Messages1)
                .WithOne(t => t.UsersTo)
                .HasForeignKey(t => t.ToUser);
        }
    }
}
