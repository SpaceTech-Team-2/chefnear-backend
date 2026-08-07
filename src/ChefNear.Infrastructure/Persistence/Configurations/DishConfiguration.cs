using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class DishConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.ToTable("Dishes");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(d => d.QuantityAvailable)
                .HasDefaultValue(0);

            builder.Property(d => d.AllergenInfo)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(d => d.Status)
                .HasDefaultValue(DishStatus.Available);

            builder.Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(d => d.DeletedAt)
                .IsRequired(false);

            
            builder.HasOne(d => d.Chef)
                .WithMany(u => u.Dishes)
                .HasForeignKey(d => d.ChefId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Category)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Images)
                .WithOne(i => i.Dish)
                .HasForeignKey(i => i.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Ingredients)
                .WithOne(i => i.Dish)
                .HasForeignKey(i => i.DishId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(d => d.Reviews)
                .WithOne(r => r.Dish)
                .HasForeignKey(r => r.DishId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.ChefId);
            builder.HasIndex(d => d.CategoryId);
            builder.HasIndex(d => d.Name);
            builder.HasIndex(d => d.Status);
            builder.HasIndex(d => d.IsDeleted);
        }
    }
}