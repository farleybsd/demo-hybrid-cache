using hybrid_cache.Api.Database.Context;
using hybrid_cache.Api.Domain;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Features.GetArticleById;

internal sealed class Handler(BlogDbContext dbContext, HybridCache cache, ILogger<Handler> logger)
{
    private static readonly HybridCacheEntryOptions CacheOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };

    public async Task<Article?> HandleAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Buscando o artigo {ArticleId}", id);

        var article = await cache.GetOrCreateAsync(
            CacheKey(id),
            (dbContext, logger, id),
            static async (state, token) =>
            {
                state.logger.LogInformation("Artigo {ArticleId} não está no cache, consultando o banco", state.id);
                return await state.dbContext.Articles.FindAsync([state.id], token);
            },
            CacheOptions,
            tags: ["articles"],
            cancellationToken: ct
        );

        if (article is null)
            logger.LogWarning("Artigo {ArticleId} não encontrado", id);

        return article;
    }

    private static string CacheKey(int id) => $"article:{id}";
}
