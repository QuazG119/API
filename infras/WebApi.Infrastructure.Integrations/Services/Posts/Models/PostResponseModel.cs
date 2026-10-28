namespace WebApi.Infrastructure.Integrations.Services.Posts.Models;

public class PostResponseModel
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}
