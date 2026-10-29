using System.Data;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using WebApi.Infrastructure.Repositories.Configurations;

namespace WebApi.Infrastructure.Repositories.Connections;

public class ConnectionFactory : IConnection
{
    private readonly ConnectionConfigs configs;

    public ConnectionFactory(IOptions<ConnectionConfigs> options) => configs = options.Value;

    public IDbConnection DbConnection(DbConnectionType type, string connectionName)
    {
        var property = typeof(ConnectionConfigs).GetProperty(connectionName)
            ?? throw new InvalidOperationException($"Connection '{connectionName}' chưa được khai báo.");

        var connectionString = property.GetValue(configs) as string;
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Connection string '{connectionName}' đang trống.");

        return type switch
        {
            DbConnectionType.Oracle => new OracleConnection(connectionString),
            _ => throw new NotSupportedException($"Provider '{type}' chưa được hỗ trợ.")
        };
    }
}
