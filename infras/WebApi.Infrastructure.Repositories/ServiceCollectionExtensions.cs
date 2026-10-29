using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Application.Abstractions;
using WebApi.Infrastructure.Repositories.Configurations;
using WebApi.Infrastructure.Repositories.Connections;
using WebApi.Infrastructure.Repositories.Implements.ReturnResults.Queries;

namespace WebApi.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
    {

        var section = configuration.GetSection("ConnectionStrings");
        services.Configure<ConnectionConfigs>(section);


        var configs = section.Get<ConnectionConfigs>();
        if (string.IsNullOrWhiteSpace(configs?.PLSQLServerPartnerWrite))
            throw new InvalidOperationException(
                "Thiếu ConnectionStrings:PLSQLServerPartnerWrite trong cấu hình.");


        DefaultTypeMap.MatchNamesWithUnderscores = true;


        services.AddSingleton<IConnection, ConnectionFactory>();


        services.AddScoped<IReturnResultQueryRepository, ReturnResultQueryRepository>();

        return services;
    }
}
