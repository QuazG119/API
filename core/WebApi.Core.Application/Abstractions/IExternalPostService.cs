using WebApi.Core.Application.Abstractions.Models;
namespace WebApi.Core.Application.Abstractions;

public interface IExternalPostService
{
    Task<ExternalPostDto?> GetPostByIdAsync(int id, CancellationToken cancellationToken);
}
