using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
    {
        public void Configure(EntityTypeBuilder<Dispute> builder)
        {
            builder.ToTable("Disputes");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Reason)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(d => d.Resolution)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.Status)
                .HasDefaultValue(DisputeStatus.Open);

            builder.HasOne(d => d.Order)
                .WithOne(o => o.Dispute)
                .HasForeignKey<Dispute>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.FiledBy)
                .WithMany(u => u.FiledDisputes)
                .HasForeignKey(d => d.FiledByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ResolvedByAdmin)
                .WithMany(u => u.ResolvedDisputes)
                .HasForeignKey(d => d.ResolvedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.OrderId)
                .IsUnique();

            builder.HasIndex(d => d.FiledByUserId);
            builder.HasIndex(d => d.ResolvedByAdminId);
            builder.HasIndex(d => d.Status);
        }
    }
}