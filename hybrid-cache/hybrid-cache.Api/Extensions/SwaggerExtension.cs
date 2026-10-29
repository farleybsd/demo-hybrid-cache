using FastEndpoints.OpenApi;

namespace hybrid_cache.Api.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.OpenApiDocument(options =>
        {
            options.DocumentName = "v1";
            options.Title = "Hybrid Cache API";
            options.Version = "v1";
        });

        return services;
    }

    public static WebApplication UseSwaggerConfig(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Hybrid Cache API v1");
        });

        return app;
    }
}
