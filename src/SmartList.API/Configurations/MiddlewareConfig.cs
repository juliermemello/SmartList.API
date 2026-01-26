using Ardalis.GuardClauses.Net9;
using SmartList.API.Middleware;

namespace SmartList.API.Configurations;

public static class MiddlewareConfig
{
    public static void AddAPIMiddleware(this IApplicationBuilder app)
    {
        Guard.Against.Null(app, nameof(app));

        app.UseMiddleware<ExceptionMiddleware>();
    }
}
