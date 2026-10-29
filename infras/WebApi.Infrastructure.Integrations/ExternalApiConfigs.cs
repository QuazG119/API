namespace WebApi.Infrastructure.Integrations.Configurations;

public class ExternalApiConfigs
{
    public const string SectionName = "ExternalApis:JsonPlaceholder";

    public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}
