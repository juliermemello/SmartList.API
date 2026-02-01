using Ardalis.GuardClauses.Net9;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application;
using SmartList.Domain.Models;
using SmartList.Infrastructure;
using System.Text.Json;

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
        
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();

                    var validationErrors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var errorDetails = JsonSerializer.Serialize(validationErrors);
                    
                    logger.LogWarning("Falha de validação na requisição {Path}: {Errors}",
                        context.HttpContext.Request.Path,
                        errorDetails);

                    var errorResponse = new ErrorResponse
                    {
                        Message = "Erro de validação nos dados enviados.",
                        Errors = validationErrors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

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
