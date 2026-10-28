using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Application.Abstractions;
using WebApi.Infrastructure.Integrations.Configurations;
using WebApi.Infrastructure.Integrations.Services.Posts.Implements;

namespace WebApi.Infrastructure.Integrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegisterIntegrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var configs = configuration.GetSection(ExternalApiConfigs.SectionName).Get<ExternalApiConfigs>();
        if (configs is null || string.IsNullOrWhiteSpace(configs.BaseUrl))
            throw new InvalidOperationException($"Thiếu cấu hình {ExternalApiConfigs.SectionName}:BaseUrl.");

        services.AddHttpClient<IExternalPostService, PostService>(client =>
        {
            client.BaseAddress = new Uri(configs.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(configs.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}
