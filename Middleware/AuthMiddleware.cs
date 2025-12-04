namespace CrudStudents.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY = "mi-api-key-secreta";

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Permitir Swagger sin auth
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-Key", out var key))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new 
            { 
                message = "API Key requerida en header X-API-Key" 
            });
            return;
        }

        if (key != API_KEY)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new 
            { 
                message = "API Key inválida" 
            });
            return;
        }

        await _next(context);
    }
}