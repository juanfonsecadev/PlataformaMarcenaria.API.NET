using PlataformaMarcenaria.API.Exceptions;
using System.Net;

namespace PlataformaMarcenaria.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            if (ex.InnerException != null)
            {
                _logger.LogError(ex.InnerException, "Inner exception: {Message}", ex.InnerException.Message);
            }
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ResourceNotFoundException => HttpStatusCode.NotFound,
            BusinessException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        // Incluir exceção interna para debug
        var errorMessage = exception.Message;
        if (exception.InnerException != null)
        {
            errorMessage += $" | Inner Exception: {exception.InnerException.Message}";
        }

        var response = new
        {
            error = errorMessage,
            statusCode = (int)statusCode,
            details = exception.InnerException?.Message ?? exception.Message
        };

        // Adicionar stackTrace apenas em desenvolvimento
        if (_environment.IsDevelopment())
        {
            var responseWithStackTrace = new
            {
                error = errorMessage,
                statusCode = (int)statusCode,
                details = exception.InnerException?.Message ?? exception.Message,
                stackTrace = exception.StackTrace,
                innerExceptionStackTrace = exception.InnerException?.StackTrace
            };
            return context.Response.WriteAsJsonAsync(responseWithStackTrace);
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

