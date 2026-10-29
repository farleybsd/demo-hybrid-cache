using hybrid_cache.Api.Database.Context;
using hybrid_cache.Api.Domain;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Features.CreateArticle;

internal sealed class Handler(BlogDbContext dbContext, HybridCache cache, ILogger<Handler> logger)
{
    public async Task<Article> HandleAsync(ArticleInput input, CancellationToken ct)
    {
        var article = new Article
        {
            Title = input.Title,
            Slug = input.Slug,
            Content = input.Content,
            CategoryId = input.CategoryId
        };

        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Artigo {ArticleId} criado", article.Id);

        // Invalida a lista e os artigos individuais que estão em cache
        await cache.RemoveByTagAsync("articles", ct);

        return article;
    }
}
