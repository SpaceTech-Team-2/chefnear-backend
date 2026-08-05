using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasDefaultValue(PaymentStatus.Pending);

            builder.Property(p => p.GatewayTransactionId)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(p => p.PaidAt)
                .IsRequired(false);

            builder.Property(p => p.HeldAt)
                .IsRequired(false);

            builder.Property(p => p.ReleasedAt)
                .IsRequired(false);

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.OrderId)
                .IsUnique();

            builder.HasIndex(p => p.Status);

            builder.HasIndex(p => p.GatewayTransactionId)
                .IsUnique();

            builder.HasIndex(p => p.IdempotencyKey)
                .IsUnique();
        }
    }
}