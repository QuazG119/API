using System.Net;
using System.Net.Http.Json;
using WebApi.Core.Application.Abstractions;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Infrastructure.Integrations.Services.Posts.Endpoints;
using WebApi.Infrastructure.Integrations.Services.Posts.Models;

namespace WebApi.Infrastructure.Integrations.Services.Posts.Implements;

public class PostService : IExternalPostService
{
    private readonly HttpClient httpClient;

    public PostService(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<ExternalPostDto?> GetPostByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(PostEndpoints.GetById(id), cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<PostResponseModel>(cancellationToken: cancellationToken);
        if (model is null)
            return null;

        return new ExternalPostDto
        {
            Id = model.Id,
            UserId = model.UserId,
            Title = model.Title ?? string.Empty,
            Body = model.Body ?? string.Empty
        };
    }
}
