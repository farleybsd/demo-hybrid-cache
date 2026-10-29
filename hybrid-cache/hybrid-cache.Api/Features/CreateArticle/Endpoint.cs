using FastEndpoints;
using hybrid_cache.Api.Domain;

namespace hybrid_cache.Api.Features.CreateArticle;

internal sealed class Endpoint(Handler handler) : Endpoint<ArticleInput, Article>
{
    public override void Configure()
    {
        Post("/CreateArticle");
        AllowAnonymous();
    }

    public override async Task<FastEndpoints.Void> HandleAsync(ArticleInput req, CancellationToken ct)
    {
        var article = await handler.HandleAsync(req, ct);

        return await Send.CreatedAtAsync<GetArticleById.Endpoint>(new { id = article.Id }, article, cancellation: ct);
    }
}
