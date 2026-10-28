namespace WebApi.Core.Application.Abstractions.Models;

public class DeploymentHistoryDto
{
    public long Id { get; set; }
    public long SupId { get; set; }
    public string? Description { get; set; }
    public string? AccountModify { get; set; }
    public int Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}
