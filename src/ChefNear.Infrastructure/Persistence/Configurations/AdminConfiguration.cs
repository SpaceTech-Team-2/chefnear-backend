using ChefNear.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasMany(a => a.ResolvedDisputes)
                .WithOne(d => d.ResolvedByAdmin)
                .HasForeignKey(d => d.ResolvedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
