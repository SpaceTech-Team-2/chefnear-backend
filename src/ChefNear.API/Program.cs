using Asp.Versioning.ApiExplorer;
using ChefNear.API;
using ChefNear.API.Middlewares;
using ChefNear.Application;
using ChefNear.Infrastructure;
using ChefNear.Shared;

var builder = WebApplication.CreateBuilder(args);

// Register Clean Architecture Layer Services
builder.Services.AddSharedServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

// Global Exception Handling Middleware
app.UseGlobalExceptionMiddleware();

// Configure the HTTP request pipeline.
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
