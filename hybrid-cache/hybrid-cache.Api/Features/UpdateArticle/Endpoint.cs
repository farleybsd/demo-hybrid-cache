using FastEndpoints;

namespace hybrid_cache.Api.Features.UpdateArticle;

internal sealed class Endpoint(Handler handler) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("/UpdateArticle/{id}");
        AllowAnonymous();
    }

    public override async Task<FastEndpoints.Void> HandleAsync(Request request, CancellationToken ct)
    {
        var updated = await handler.HandleAsync(request, ct);

        if (!updated)
            return await Send.NotFoundAsync(ct);

        return await Send.NoContentAsync(ct);
    }
}
