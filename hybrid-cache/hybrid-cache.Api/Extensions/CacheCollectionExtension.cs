using hybrid_cache.Api.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace hybrid_cache.Api.Extensions;

public static class CacheCollectionExtension
{
    public static IServiceCollection AddHybridCacheConfig(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
        {
            options.MaximumKeyLength = 256;
            options.MaximumPayloadBytes = 5 * 1024 * 1024;
            options.ReportTagMetrics = false;

            options.DefaultEntryOptions = new HybridCacheEntryOptions()
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(5),
                Expiration = TimeSpan.FromMinutes(10)
            };
        })
        .AddSerializerFactory(new ReadableJsonSerializerFactory());

        return services;
    }

    // Deve ser chamado depois do AddRedis: decora o IDistributedCache registrado por ele
    public static IServiceCollection AddReadableCacheMirror(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis")!;

        // Uma única conexão, compartilhada pelo RedisCache e pela cópia legível
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(connectionString));
        services.AddOptions<RedisCacheOptions>()
            .PostConfigure<IConnectionMultiplexer>((options, redis) =>
                options.ConnectionMultiplexerFactory = () => Task.FromResult(redis));

        var redisCache = services.Last(d => d.ServiceType == typeof(IDistributedCache));
        services.Remove(redisCache);

        services.AddSingleton<IDistributedCache>(sp =>
        {
            var inner = (IDistributedCache)(redisCache.ImplementationInstance
                ?? redisCache.ImplementationFactory?.Invoke(sp)
                ?? ActivatorUtilities.CreateInstance(sp, redisCache.ImplementationType!));

            return new ReadableCacheMirror(
                inner,
                sp.GetRequiredService<IConnectionMultiplexer>(),
                sp.GetRequiredService<ILogger<ReadableCacheMirror>>());
        });

        return services;
    }
}
