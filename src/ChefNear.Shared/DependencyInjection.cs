using Microsoft.Extensions.DependencyInjection;

namespace ChefNear.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        return services;
    }
}
