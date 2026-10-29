using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning.ApiExplorer;
using APITest.Domains;
using APITest.Middlewares;
using WebApi.Core.Application;
using WebApi.Infrastructure.Integrations;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Localhost"))
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}
else
{
    builder.Configuration.AddJsonFile("/vault/secrets/appsettings.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddEnvironmentVariables();
    builder.Configuration.AddCommandLine(args);
}
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Logging.AddConsole();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSharedServices();

var configuration = builder.Configuration;
builder.Services.AddServices(configuration);
builder.Services.AddRegisterIntegrationServices(configuration);
builder.Services.AddRegisterRepositories(configuration);
builder.Services.AddCoreApplication(configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var d in provider.ApiVersionDescriptions)
        options.SwaggerEndpoint($"/swagger/{d.GroupName}/swagger.json", d.GroupName.ToUpperInvariant());
});
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
