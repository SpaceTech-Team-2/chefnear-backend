using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChefNear.Domain.Entities;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Label)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.City)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Details)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(a => a.Latitude)
                .HasPrecision(10, 7)
                .IsRequired();

            builder.Property(a => a.Longitude)
                .HasPrecision(10, 7)
                .IsRequired();

            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false);

            
            builder.HasOne(a => a.Client)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Orders)
                .WithOne(o => o.DeliveryAddress)
                .HasForeignKey(o => o.DeliveryAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.ClientId);
            builder.HasIndex(a => a.IsDefault);
        }
    }
}