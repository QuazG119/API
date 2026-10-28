using System.Data;
using Dapper;
using Dapper.Oracle;
using WebApi.Core.Application.Abstractions;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Infrastructure.Repositories.Configurations;
using WebApi.Infrastructure.Repositories.Connections;

namespace WebApi.Infrastructure.Repositories.Implements.ReturnResults.Queries;

public class ReturnResultQueryRepository : IReturnResultQueryRepository
{
    private readonly IConnection connection;

    public ReturnResultQueryRepository(IConnection connection) => this.connection = connection;

    public async Task<IReadOnlyList<DeploymentHistoryDto>> GetHistoryNotOkAsync(
        long supId, CancellationToken cancellationToken)
    {
        using IDbConnection dbConnection = connection.DbConnection(
            DbConnectionType.Oracle,
            nameof(ConnectionConfigs.PLSQLServerPartnerWrite));

        var parameters = new OracleDynamicParameters();
        parameters.Add("p_SupId", supId, OracleMappingType.Int64, ParameterDirection.Input);
        parameters.Add("p_result", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        dbConnection.Open();

        var result = await dbConnection.QueryAsync<DeploymentHistoryDto>(
            new CommandDefinition(
                commandText: "PAR.OSU4R_TKBTv2_Deployment_GetHistoryDeploymentNotOk",
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.AsList();
    }
}
