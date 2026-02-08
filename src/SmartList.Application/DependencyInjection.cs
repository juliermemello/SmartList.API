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
            cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAxNDQwMDAwIiwiaWF0IjoiMTc2OTkxMDQ2OSIsImFjY291bnRfaWQiOiIwMTljMTZlMjEyZTU3OTZkODg5ZGNkNGJlMjIwYWI4MCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2diZTUxM2FtMXk4eTB2eDc3ZnM3aDZ3Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.m3Du9p1TUNip3WnoCxwvQ7h0QSUS3e50gX6g5d5MyyLc394Ql8nCdBXQsL_zcD27Qj7exJkrJtIkLmBBBRZ_hYO4fGRlse5gEhxja9HEAw7ZkMUw6nBwo2DUsFN4qmMDFVXRgz7WFztws1zrzzDvvZr-2xqDu3ftHWCEUsg2susa62xiPXiluezyOYkSK__jkWml75FgB6YSMGKsZcZnCxkUfUAUmDpVYg8Gquc-t_wToL26bFulC6cXZW-20v__pV7txT6cbuQWw6KsC10oPnO3FJM5tbGpGkGRi9jFS3XfHkHfUFZChxSu5WpsX1s6H2DgtUqU-C65kmXNHl5sKQ";

            cfg.AddMaps(typeof(UserMappingProfile).Assembly);
            cfg.AddMaps(typeof(CategoryMappingProfile).Assembly);
            cfg.AddMaps(typeof(ProductMappingProfile).Assembly);
            cfg.AddMaps(typeof(ShoppingListMappingProfile).Assembly);
            cfg.AddMaps(typeof(ShoppingListItemMappingProfile).Assembly);
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
        services.AddScoped<IShoppingListService, ShoppingListService>();
        services.AddScoped<IShoppingListItemService, ShoppingListItemService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
