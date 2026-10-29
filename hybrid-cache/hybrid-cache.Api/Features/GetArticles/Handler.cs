using hybrid_cache.Api.Database.Context;
using hybrid_cache.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Features.GetArticles;

internal sealed class Handler(BlogDbContext dbContext, HybridCache cache)
{
    private const string CacheKey = "articles";

    public async Task<IReadOnlyList<ArticleResponse>> HandleAsync(CancellationToken ct)
    {
        return await cache.GetOrCreateAsync(
            CacheKey,
            async token =>
            {
                var articles = await dbContext.Articles.AsNoTracking().ToListAsync(token);
                return articles.Select(Mapear).ToList();
            },
            tags: ["articles"],
            cancellationToken: ct
        );
    }

    internal static ArticleResponse Mapear(Article article) => new()
    {
        Id = article.Id,
        CategoryId = article.CategoryId,
        Title = article.Title,
        Slug = article.Slug,
        Content = article.Content,
        CreatedAt = article.CreatedAt,
        UpdatedAt = article.UpdatedAt
    };
}
