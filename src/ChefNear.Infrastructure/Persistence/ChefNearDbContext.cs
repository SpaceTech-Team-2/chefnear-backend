using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence.Configurations;

namespace ChefNear.Infrastructure.Persistence
{
    public class ChefNearDbContext : IdentityDbContext<User>
    {
        public ChefNearDbContext(DbContextOptions<ChefNearDbContext> options)
            : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }  
        public DbSet<DeviceToken> DeviceTokens { get; set; }

        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishImage> DishImages { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> Transactions { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configuration files
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChefNearDbContext).Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<Enum>()
                .HaveConversion<string>();
        }
    }
}