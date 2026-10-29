namespace WebApi.Infrastructure.Integrations.Services.Posts.Endpoints;

public static class PostEndpoints
{
    public static string GetById(int id) => $"posts/{id}";
}
