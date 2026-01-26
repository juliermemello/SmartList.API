using Ardalis.GuardClauses.Net9;
using Serilog;
using SmartList.Application;
using SmartList.Infrastructure;

namespace SmartList.API.Configurations;

public static class GeneralConfiguration
{
    public static void AddGeneralConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(builder, nameof(builder));

        services.AddLog(builder);

        services.AddControllers();
        
        services.AddEndpointsApiExplorer();

        services.AddInfrastructure(builder.Configuration);
        
        services.AddApplication(builder.Configuration);

        services.AddSwaggerConfiguration();

        services.AddAPICors();

        services.AddHealthCheckConfiguration();
    }

    public static void AddGeneralApp(this IApplicationBuilder app)
    {
        Guard.Against.Null(app, nameof(app));

        app.UseSerilogRequestLogging();
        app.AddAPICors();
        app.AddSwagger();
        app.AddAPIMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRouting();
        app.AddHealthCheck();
    }
}
