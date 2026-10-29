using FastEndpoints;
using hybrid_cache.Api.Domain;

namespace hybrid_cache.Api.Features.GetArticleById;

internal sealed class Endpoint(Handler handler) : Endpoint<Request, Article>
{
    public override void Configure()
    {
        Get("/GetArticleById/{id}");
        AllowAnonymous();
    }

    public override async Task<FastEndpoints.Void> HandleAsync(Request request, CancellationToken ct)
    {
        var article = await handler.HandleAsync(request.Id, ct);

        if (article is null)
            return await Send.NotFoundAsync(ct);

        return await Send.OkAsync(article, ct);
    }
}
