using ChefNear.Domain.Exceptions;
using ChefNear.Shared.Responses;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace ChefNear.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request execution: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;
        List<string> errors = new();

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Validation failure(s) occurred.";
                errors = validationEx.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
                break;

            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message ?? "The requested resource was not found.";
                if (!string.IsNullOrEmpty(exception.Message)) errors.Add(exception.Message);
                break;

            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = "Unauthorized access.";
                if (!string.IsNullOrEmpty(exception.Message)) errors.Add(exception.Message);
                break;

            case ArgumentException or InvalidOperationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                if (!string.IsNullOrEmpty(exception.Message)) errors.Add(exception.Message);
                break;

            case PaymentGatewayException:
                statusCode = ((PaymentGatewayException)exception).StatusCode;
                message = "Failed to process payment.";
                if (!string.IsNullOrEmpty(exception.Message)) errors.Add(exception.Message);
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = _env.IsDevelopment()
                    ? exception.Message
                    : "An internal server error occurred. Please try again later.";
                if (_env.IsDevelopment() && exception.StackTrace != null)
                {
                    errors.Add(exception.StackTrace);
                }
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = ApiResponse.FailureResponse(message, errors, statusCode);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
