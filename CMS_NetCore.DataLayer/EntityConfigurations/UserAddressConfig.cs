using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS_NetCore.DataLayer.EntityConfigurations;

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> modelBuilder)
    {
        modelBuilder.Property(userAddress => userAddress.City).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.HomeNo).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.MobileNo).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.PostalCode).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.PostAddress).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.State).HasMaxLength(100);
        modelBuilder.Property(userAddress => userAddress.NameFamily).HasMaxLength(100);

        modelBuilder.HasOne(userAddress => userAddress.Users)
            .WithMany(user => user.UserAddresses)
            .HasForeignKey(userAddress => new { userAddress.UserId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}