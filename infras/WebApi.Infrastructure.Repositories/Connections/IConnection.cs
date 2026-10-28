using System.Data;

namespace WebApi.Infrastructure.Repositories.Connections;

public enum DbConnectionType
{
    Oracle
}

public interface IConnection
{
    /// <summary>Tạo IDbConnection mới (chưa mở). Người gọi chịu trách nhiệm Dispose.</summary>
    IDbConnection DbConnection(DbConnectionType type, string connectionName);
}
