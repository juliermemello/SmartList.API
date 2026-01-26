using Ardalis.GuardClauses.Net9;

namespace SmartList.API.Configurations;

public static class CorsConfig
{
    private static string CorsPolicy = "api-policy";

    public static void AddAPICors(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy,
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    public static void AddAPICors(this IApplicationBuilder app)
    {
        Guard.Against.Null(app, nameof(app));
        
        app.UseCors(CorsPolicy);
    }
}
