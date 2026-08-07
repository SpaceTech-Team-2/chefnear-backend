using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChefNear.Domain.Entities;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.DisplayName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(u => u.PhotoUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasMany(u => u.FiledDisputes)
                .WithOne(d => d.FiledBy)
                .HasForeignKey(d => d.FiledByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}