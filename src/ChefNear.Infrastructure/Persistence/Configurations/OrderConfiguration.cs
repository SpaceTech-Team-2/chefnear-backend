using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.CancellationReason)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.CancellationReasonType)
                .IsRequired(false);

            builder.Property(o => o.IsActive)
                .HasComputedColumnSql("""
                    CASE
                        WHEN [Status] <> 'Delivered'
                         AND [Status] <> 'Cancelled'
                         AND [DeliveredAt] IS NULL
                         AND [CanceledAt] IS NULL
                        THEN CAST(1 AS bit)
                        ELSE CAST(0 AS bit)
                    END
                    """
                ); // o.Status < OrderStatus.Delivered && o.DeliveredAt == null && o.CanceledAt == null

            // Relationships

            // Order (M) → User (1) (Client)
            builder.HasOne(o => o.Client)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order (1) → OrderItem (M)
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order (M) → Address (1)
            builder.HasOne(o => o.DeliveryAddress)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.DeliveryAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order (1) → Payment (1)
            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order (1) → Reviews (Many) 
            builder.HasMany(o => o.Reviews)
                .WithOne(r => r.Order)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Dispute)
                .WithOne(d => d.Order)
                .HasForeignKey<Dispute>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Notifications)
                .WithOne(n => n.Order)
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.ClientId);
            builder.HasIndex(o => o.DeliveryAddressId);
            builder.HasIndex(o => o.Status);
        }
    }
}