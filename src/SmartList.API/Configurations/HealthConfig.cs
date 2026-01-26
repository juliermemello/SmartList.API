using Ardalis.GuardClauses.Net9;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SmartList.Infrastructure.Context;

namespace SmartList.API.Configurations;

public static class HealthConfig
{
    public static void AddHealthCheckConfiguration(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<AppDbContext>(name: "SQL Server")
            .AddProcessAllocatedMemoryHealthCheck(
                maximumMegabytesAllocated: 512,
                name: "Memory_Check",
                tags: new[] { "system" }
            );

        services.AddHealthChecksUI(setup =>
        {
            setup.AddHealthCheckEndpoint("API", "/health");
            setup.SetEvaluationTimeInSeconds(30);
        }).AddInMemoryStorage();
    }

    public static void AddHealthCheck(this IApplicationBuilder app)
    {
        Guard.Against.Null(app, nameof(app));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(setup => setup.UIPath = "/health-ui");
        });
    }
}
