using WebApi.Core.Application.Abstractions.Models;
namespace WebApi.Core.Application.Abstractions;

public interface IReturnResultQueryRepository
{
    Task<IReadOnlyList<DeploymentHistoryDto>> GetHistoryNotOkAsync(long supId, CancellationToken cancellationToken);
}
