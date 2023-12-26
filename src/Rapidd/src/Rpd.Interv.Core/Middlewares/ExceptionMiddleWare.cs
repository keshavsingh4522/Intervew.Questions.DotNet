using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Rpd.Interv.Core.Middlewares;

/// <summary>
/// Middle-ware to handle exceptions globally and log them.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionMiddleWare"/> class.
/// </remarks>
/// <param name="next">The next middle-ware in the pipeline.</param>
/// <param name="logger">The logger used for logging exceptions.</param>
public class ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleWare> _logger = logger;

    /// <summary>
    /// Invokes the middleware operation.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Proceed with the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "Unhandled exception occurred.");

            // Handle the exception and write an error response
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception and writes an error response.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set the response content type to application/json
        context.Response.ContentType = "application/json";

        // Set the status code to 500 Internal Server Error
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Construct a result object with the error details
        var result = new
        {
            context.Response.StatusCode,
            Message = "An unexpected error occurred. Please try again later.",
            // Exclude detailed exception information in a production environment for security
            Detailed = exception.Message // Consider removing or securing this in production
        };

        // Serialize the result object to JSON and write it to the response
        return context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}