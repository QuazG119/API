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
        // b1. Đăng ký ConnectionConfigs từ section ConnectionStrings.
        var section = configuration.GetSection("ConnectionStrings");
        services.Configure<ConnectionConfigs>(section);

        // b3. Kiểm tra connection string bắt buộc: thiếu thì dừng ngay khi khởi động.
        var configs = section.Get<ConnectionConfigs>();
        if (string.IsNullOrWhiteSpace(configs?.PLSQLServerPartnerWrite))
            throw new InvalidOperationException(
                "Thiếu ConnectionStrings:PLSQLServerPartnerWrite trong cấu hình.");

        // Cột Oracle dạng SUP_ID tự ánh xạ vào property SupId.
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        // b2. Connection factory dùng chung.
        services.AddSingleton<IConnection, ConnectionFactory>();

        // b4. Repositories.
        services.AddScoped<IReturnResultQueryRepository, ReturnResultQueryRepository>();

        return services;
    }
}
