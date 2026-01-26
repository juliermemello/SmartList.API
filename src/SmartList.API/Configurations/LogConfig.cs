using Ardalis.GuardClauses.Net9;
using Serilog;

namespace SmartList.API.Configurations;

public static class LogConfig
{
    public static void AddLog(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(builder, nameof(builder));

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
