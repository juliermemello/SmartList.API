using Ardalis.GuardClauses.Net9;
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
        services.AddInfrastructure(builder.Configuration);
        services.AddApplication(builder.Configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfiguration();
        services.AddJWTConfiguration(builder.Configuration);
        services.AddAPICors();
        services.AddHealthCheckConfiguration();
        services.AddApiVersioning();
        services.AddRouting(options => {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        services.AddHttpContextAccessor();
    }
}
