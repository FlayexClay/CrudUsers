using System.Diagnostics;

namespace CrudStudents.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("=> {Method} {Path}", method, path);

        await _next(context);

        sw.Stop();
        _logger.LogInformation("<= {Method} {Path} {StatusCode} ({ElapsedMs}ms)",
            method, path, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}