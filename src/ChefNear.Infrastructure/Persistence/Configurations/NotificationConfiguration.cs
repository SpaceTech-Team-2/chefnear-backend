using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(n => n.Status)
                .HasDefaultValue(NotificationStatus.Pending);

            builder.Property(n => n.SentAt)
                .IsRequired(false);

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Order)
                .WithMany(o => o.Notifications)
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(n => n.UserId);
            builder.HasIndex(n => n.OrderId);
            builder.HasIndex(n => n.Status);
        }
    }
}