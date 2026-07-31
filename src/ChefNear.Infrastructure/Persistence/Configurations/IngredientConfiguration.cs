using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChefNear.Domain.Entities;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.Quantity)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasOne(i => i.Dish)
                .WithMany(d => d.Ingredients)
                .HasForeignKey(i => i.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(i => i.DishId);
        }
    }
}