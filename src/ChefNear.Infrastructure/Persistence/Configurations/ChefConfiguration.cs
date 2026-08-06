using ChefNear.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class ChefConfiguration : IEntityTypeConfiguration<Chef>
    {
        public void Configure(EntityTypeBuilder<Chef> builder)
        {
            builder.ToTable("Chefs");

            builder.Property(c => c.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(c => c.ReliabilityScore)
                .HasPrecision(3, 2)
                .IsRequired(false);

            builder.HasOne(c => c.KitchenAddress)
                .WithMany()
                .HasForeignKey(c => c.KitchenAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Dishes)
                .WithOne(d => d.Chef)
                .HasForeignKey(d => d.ChefId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Wallet)
                .WithOne(w => w.Chef)
                .HasForeignKey<Wallet>(w => w.ChefId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
