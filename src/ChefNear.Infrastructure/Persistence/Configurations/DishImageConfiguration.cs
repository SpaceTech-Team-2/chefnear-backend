using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChefNear.Domain.Entities;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class DishImageConfiguration : IEntityTypeConfiguration<DishImage>
    {
        public void Configure(EntityTypeBuilder<DishImage> builder)
        {
            builder.ToTable("DishImages");

            builder.HasKey(di => di.Id);

            builder.Property(di => di.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(di => di.IsPrimary)
                .HasDefaultValue(false);

            builder.Property(di => di.DisplayOrder)
                .HasDefaultValue(0);

          builder.HasOne(di => di.Dish)
                .WithMany(d => d.Images)
                .HasForeignKey(di => di.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(di => di.DishId);
            builder.HasIndex(di => di.IsPrimary);
        }
    }
}