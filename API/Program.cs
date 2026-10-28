using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning.ApiExplorer;
using APITest.Domains;
using APITest.Middlewares;
using WebApi.Core.Application;
using WebApi.Infrastructure.Integrations;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.Shared;

// b1. Tạo builder.
var builder = WebApplication.CreateBuilder(args);

// b2. Nạp cấu hình theo môi trường.
// Mặc định host đã đọc appsettings.json, appsettings.{Env}.json, User Secrets (Development),
// biến môi trường và command line. Ngoài Localhost, đọc thêm file secret do Vault inject.
if (!builder.Environment.IsEnvironment("Localhost"))
{
    builder.Configuration.AddJsonFile("/vault/secrets/appsettings.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddEnvironmentVariables(); // giữ ưu tiên biến môi trường
    builder.Configuration.AddCommandLine(args);
}

// b3. MVC controllers, JSON, logging, cache, HttpContextAccessor.
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Logging.AddConsole();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSharedServices();   // MemoryCache + ICacheService

// b4. Đăng ký từng nhóm dịch vụ.
var configuration = builder.Configuration;
builder.Services.AddServices(configuration);                    // versioning, swagger, health check
builder.Services.AddRegisterIntegrationServices(configuration); // HttpClientFactory
builder.Services.AddRegisterRepositories(configuration);        // Oracle + Dapper
builder.Services.AddCoreApplication(configuration);             // MediatR + FluentValidation
// b5. Build, dựng pipeline, map controllers.
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
