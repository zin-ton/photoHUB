using System.Net;
using System.Text.Json;

namespace PhotoHUB.configs;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message    = "Something went wrong";
            
            if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message    = "Unauthorized";
            }
            else if (ex is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message    = "Invalid request";
            }
            else if (ex is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message    = "Resource not found";
            }

            context.Response.StatusCode  = statusCode;
            context.Response.ContentType = "application/json";

            var response = new { error = message };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}