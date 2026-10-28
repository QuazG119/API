using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Application.Behaviors;

namespace WebApi.Core.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Scoped);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
