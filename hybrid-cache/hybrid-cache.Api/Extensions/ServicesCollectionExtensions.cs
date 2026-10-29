using hybrid_cache.Api.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace hybrid_cache.Api.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddSqlServerDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BlogDb");

        services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        }
        );

        return services;
    }

    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<Features.GetArticles.Handler>();
        services.AddScoped<Features.GetArticleById.Handler>();
        services.AddScoped<Features.CreateArticle.Handler>();
        services.AddScoped<Features.UpdateArticle.Handler>();
        services.AddScoped<Features.DeleteArticle.Handler>();

        return services;
    }
}