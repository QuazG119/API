using System.Text.Json;
using FluentValidation;
using WebApi.Infrastructure.Shared.Constants;
using WebApi.Infrastructure.Shared.Models;

namespace APITest.Middlewares;

public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
            await WriteAsync(context, Response<object>.Fail(AppMessageConstant.ValidationFailed, 400, errors));
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "External API call failed");
            await WriteAsync(context, Response<object>.Fail(AppMessageConstant.ExternalServiceError, 502));
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteAsync(context, Response<object>.Fail(AppMessageConstant.InternalError, 500));
        }
    }

    private static async Task WriteAsync(HttpContext context, Response<object> response)
    {
        if (context.Response.HasStarted) return;
        context.Response.Clear();
        context.Response.StatusCode = response.Code;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}
