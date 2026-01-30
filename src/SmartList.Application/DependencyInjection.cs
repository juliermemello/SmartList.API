using Ardalis.GuardClauses.Net9;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartList.Application.Interfaces;
using SmartList.Application.Mappings;
using SmartList.Application.Services;
using SmartList.Domain.Models;
using System.Reflection;

namespace SmartList.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(configuration, nameof(configuration));

        services.AddApplicationAutoMapper();
        services.AddApplicationValidators();
        services.AddApplicationServices(configuration);

        return services;
    }

    private static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserMappingProfile).Assembly);
            cfg.AddMaps(typeof(CategoryMappingProfile).Assembly);
            cfg.AddMaps(typeof(ProductMappingProfile).Assembly);
        });

        return services;
    }

    private static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecuritySettings>(options =>
        {
            configuration.GetSection("SecuritySettings:Argon2").Bind(options);
        });

        services.AddSingleton<IPasswordHasher, PasswordHasherService>();

        services.AddScoped(typeof(IBaseService<,,,>), typeof(BaseService<,,,>));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
