namespace WebApi.Core.Application.Abstractions.Models;

public class ExternalPostDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
