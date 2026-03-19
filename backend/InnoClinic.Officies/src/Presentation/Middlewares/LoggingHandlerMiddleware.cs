using System.Diagnostics;

namespace Presentation.Middlewares;

public class LoggingHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingHandlerMiddleware> _logger;

    public LoggingHandlerMiddleware(RequestDelegate next, ILogger<LoggingHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var request = context.Request;

        _logger.LogInformation("HTTP Request Started: {Method} {Path}", request.Method, request.Path);

        try
        {
            await _next(context);
            sw.Stop();

            _logger.LogInformation(
                "HTTP Request Finished: {Method} {Path} responded {StatusCode} in {Elapsed:0.0000} ms",
                request.Method,
                request.Path,
                context.Response.StatusCode,
                sw.Elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "HTTP Request Failed: {Method} {Path} after {Elapsed:0.0000} ms",
                request.Method, request.Path, sw.Elapsed.TotalMilliseconds);
            throw;
        }
    }
}
