using Ardalis.GuardClauses.Net9;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SmartList.API.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        Guard.Against.Null(services, nameof(services));

        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SmartList API",
                Version = "v1",
                Description = "O SmartList é o seu assistente inteligente de compras, projetado para transformar a desorganização do mercado em uma experiência rápida e estratégica. Mais do que uma simples lista, o sistema utiliza inteligência para prever suas necessidades, organizar itens por categoria e otimizar seu tempo no corredor. Com o SmartList, você compra exatamente o que precisa, sem esquecimentos e sem desperdícios.",
                Contact = new OpenApiContact
                {
                    Name = "Julierme Pereira Mello",
                    Email = "juliermemello@gmail.com"
                }
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu_token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            s.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static void AddSwagger(this IApplicationBuilder app)
    {
        Guard.Against.Null(app, nameof(app));

        app.UseSwagger();

        app.UseSwaggerUI(s =>
        {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartList API v1");
        });
    }
}
