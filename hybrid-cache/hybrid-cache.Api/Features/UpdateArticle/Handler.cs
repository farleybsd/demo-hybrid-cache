using hybrid_cache.Api.Database.Context;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Features.UpdateArticle;

internal sealed class Handler(BlogDbContext dbContext, HybridCache cache, ILogger<Handler> logger)
{
    public async Task<bool> HandleAsync(Request request, CancellationToken ct)
    {
        var article = await dbContext.Articles.FindAsync([request.Id], ct);

        if (article is null)
        {
            logger.LogWarning("Artigo {ArticleId} não encontrado para atualização", request.Id);
            return false;
        }

        article.Title = request.Title;
        article.Slug = request.Slug;
        article.Content = request.Content;
        article.CategoryId = request.CategoryId;
        article.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Artigo {ArticleId} atualizado", article.Id);

        // Invalida a lista e os artigos individuais que estão em cache
        await cache.RemoveByTagAsync("articles", ct);

        return true;
    }
}
