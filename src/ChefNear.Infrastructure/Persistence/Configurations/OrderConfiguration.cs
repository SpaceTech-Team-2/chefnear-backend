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

            builder.Property(o => o.Quantity)
                .HasDefaultValue(1)
                .IsRequired();

            builder.Property(o => o.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.CancellationReason)
                .HasMaxLength(500)
                .IsRequired(false);

            // العلاقات

            // Order (M) → User (1) (Client)
            builder.HasOne(o => o.Client)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order (M) → Dish (1)
            builder.HasOne(o => o.Dish)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DishId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Order (1) → Review (1)
            builder.HasOne(o => o.Review)
                .WithOne(r => r.Order)
                .HasForeignKey<Review>(r => r.OrderId)
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
            builder.HasIndex(o => o.DishId);
            builder.HasIndex(o => o.DeliveryAddressId);
            builder.HasIndex(o => o.Status);
        }
    }
}