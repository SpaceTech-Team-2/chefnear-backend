using ChefNear.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    internal class DeviceTokenConfiguration : IEntityTypeConfiguration<DeviceToken>
    {
        public void Configure(EntityTypeBuilder<DeviceToken> builder)
        {
            builder
                .ToTable("DeviceTokens");

            builder
                .HasKey(x => x.Id);

            builder
                .HasIndex(x => x.Token)
                .IsUnique();

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.DeviceTokens)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
