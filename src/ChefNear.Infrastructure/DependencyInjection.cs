using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Payments;
using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Jobs;
using ChefNear.Infrastructure.Payments;
using ChefNear.Infrastructure.Payments.PaymentGateways;
using ChefNear.Infrastructure.Persistence;
using ChefNear.Infrastructure.Repositories;
using ChefNear.Infrastructure.Services;
using ChefNear.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChefNear.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ChefNearDbContext>(options =>
            options.UseSqlServer(connectionString)
                   .EnableSensitiveDataLogging());
                   

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<AppUrlSettings>(configuration.GetSection("AppUrlSettings"));
        services.Configure<PaymobSettings>(configuration.GetSection("PaymentGateway:Paymob"));
        services.Configure<FrontendSettings>(configuration.GetSection("FrontendSettings"));

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ChefNearDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IChefRepo, ChefRepo>();
        services.AddScoped<IClientRepo, ClientRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IDishImageRepo, DishImageRepo>();
        services.AddScoped<IDishRepo, DishRepo>();
        services.AddScoped<IDisputeRepo, DisputeRepo>();
        services.AddScoped<IIngredientsRepo, IngredentsRepo>();
        services.AddScoped<INotificationRepo, NotificationRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IPaymentRepo, PaymentRepo>();
        services.AddScoped<IReviewRepo, ReviewRepo>();
        services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));   
        services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();
        services.AddScoped<IPaymobService, PaymobService>();
        services.AddScoped<IAddChefEarningsJob, AddChefEarningsJob>();

        return services;
    }
}