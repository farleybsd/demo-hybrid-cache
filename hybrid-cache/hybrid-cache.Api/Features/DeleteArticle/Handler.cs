using hybrid_cache.Api.Database.Context;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Features.DeleteArticle;

internal sealed class Handler(BlogDbContext dbContext, HybridCache cache, ILogger<Handler> logger)
{
    public async Task<bool> HandleAsync(int id, CancellationToken ct)
    {
        var article = await dbContext.Articles.FindAsync([id], ct);

        if (article is null)
        {
            logger.LogWarning("Artigo {ArticleId} não encontrado para exclusão", id);
            return false;
        }

        dbContext.Articles.Remove(article);
        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Artigo {ArticleId} excluído", id);

        // Invalida a lista e os artigos individuais que estão em cache
        await cache.RemoveByTagAsync("articles", ct);

        return true;
    }
}
