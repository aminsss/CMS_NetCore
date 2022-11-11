using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.MessagesFrom)
            .WithOne(message => message.UserFrom)
            .HasForeignKey(message => message.FromUser);

        builder.HasMany(user => user.MessagesTo)
            .WithOne(message => message.UserTo)
            .HasForeignKey(message => message.ToUser);
    }
}