using System.Reflection;
using ChefNear.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<Chef> Chefs => Set<Chef>();
    public DbSet<Client> Clients => Set<Client>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Table-Per-Hierarchy (TPH) for ApplicationUser, Chef, and Client
        builder.Entity<ApplicationUser>(b =>
        {
            b.HasDiscriminator<string>("UserType")
             .HasValue<ApplicationUser>("User")
             .HasValue<Chef>("Chef")
             .HasValue<Client>("Client");
        });

        // Apply all entity configurations in the Infrastructure assembly automatically
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
