using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Shared.Caching;

namespace WebApi.Infrastructure.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        return services;
    }
}
