using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.DataLayer.EntityConfigurations
{
    public class Address_UserConfig :  IEntityTypeConfiguration<Address_User>
    {
        public void Configure(EntityTypeBuilder<Address_User> modelBuilder)
        {
            modelBuilder.Property(t => t.City).HasMaxLength(100);
            modelBuilder.Property(t => t.HomeNo).HasMaxLength(100);
            modelBuilder.Property(t => t.MobileNo).HasMaxLength(100);
            modelBuilder.Property(t => t.postalCode).HasMaxLength(100);
            modelBuilder.Property(t => t.PostAddress).HasMaxLength(100);
            modelBuilder.Property(t => t.State).HasMaxLength(100);
            modelBuilder.Property(t => t.NameFamily).HasMaxLength(100);

            //Relations

            modelBuilder.HasOne(t => t.Users)
            .WithMany(r => r.Address_Users)
            .HasForeignKey(r => new { r.UserId })
            .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
