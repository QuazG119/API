using System.Data;

namespace WebApi.Infrastructure.Repositories.Connections;

public enum DbConnectionType
{
    Oracle
}

public interface IConnection
{
    IDbConnection DbConnection(DbConnectionType type, string connectionName);
}
