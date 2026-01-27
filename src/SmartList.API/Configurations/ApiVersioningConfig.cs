using Ardalis.GuardClauses.Net9;
using Asp.Versioning;

namespace SmartList.API.Configurations;

public static class ApiVersioningConfig
{
    public static void AddApiVersioning(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader()
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
