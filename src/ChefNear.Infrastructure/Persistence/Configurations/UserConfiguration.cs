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

            builder.Property(u => u.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(u => u.ReliabilityScore)
                .HasPrecision(3, 2)
                .IsRequired(false);

           
            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.KitchenAddress)
                .WithMany()
                .HasForeignKey(u => u.KitchenAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Dishes)
                .WithOne(d => d.Chef)
                .HasForeignKey(d => d.ChefId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.FiledDisputes)
                .WithOne(d => d.FiledBy)
                .HasForeignKey(d => d.FiledByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ResolvedDisputes)
                .WithOne(d => d.ResolvedByAdmin)
                .HasForeignKey(d => d.ResolvedByAdminId)
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