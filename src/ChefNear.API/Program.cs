using Asp.Versioning.ApiExplorer;
using ChefNear.API;
using ChefNear.API.Extensions;
using ChefNear.API.Middlewares;
using ChefNear.Application;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Infrastructure;
using ChefNear.Infrastructure.Hubs;
using ChefNear.Infrastructure.Persistence;
using ChefNear.Infrastructure.Seed;
using ChefNear.Shared.Responses;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting ChefNear API host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddApiServices();
    builder.Services.AddControllers()
        .AddApplicationPart(typeof(Program).Assembly)
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    var jwtSettings = new JwtSettings();
    builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    // Configure email template path
    builder.Services.Configure<EmailTemplateSettings>(options =>
    {
        options.TemplatePath = Path.Combine(builder.Environment.ContentRootPath, "EmailTemplates");
    });

    // Hangfire
    builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddHangfireServer();

    var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                context.HandleResponse(); // stops default behavior
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer
                    .Serialize(ApiResponse.FailureResponse(
                        "No access token provided",
                        "You are not authorized. Please provide a valid token.",
                        401
                    ));

                await context.Response.WriteAsync(result);
            },

            OnForbidden = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer
                    .Serialize(
                        ApiResponse.FailureResponse(
                            "You haven't perssions to access this resource",
                            "You do not have permission to access this resource.",
                            403
                        ));

                await context.Response.WriteAsync(result);
            }
        };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddSwaggerWithJwt();


    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.InitializeAsync();

        await SeedData.SeedAsync(scope.ServiceProvider);
    }


    app.UseGlobalExceptionMiddleware();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"ChefNear API {description.GroupName.ToUpperInvariant()}");
            }
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    if (app.Environment.IsDevelopment())
    {
        // Hangfire Dashboard — available in all environments
        app.UseHangfireDashboard("/hangfire");
    }

    app.UseAuthentication();  
    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<NotificationHub>("/hubs/notifications")
        .RequireAuthorization();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "ChefNear API terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}