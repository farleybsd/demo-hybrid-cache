using FastEndpoints;
using hybrid_cache.Api.Domain;

namespace hybrid_cache.Api.Features.GetArticles;

internal sealed class Endpoint(Handler handler) : EndpointWithoutRequest<IReadOnlyList<ArticleResponse>>
{
    public override void Configure()
    {
        Get("/GetArticles");
        AllowAnonymous();
    }

    public override async Task<FastEndpoints.Void> HandleAsync(CancellationToken ct)
    {
        var resultado = await handler.HandleAsync(ct);
        return await Send.OkAsync(resultado, ct);
    }
}