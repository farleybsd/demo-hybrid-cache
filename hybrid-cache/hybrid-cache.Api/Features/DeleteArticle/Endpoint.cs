using FastEndpoints;

namespace hybrid_cache.Api.Features.DeleteArticle;

internal sealed class Endpoint(Handler handler) : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("/DeleteArticle/{id}");
        AllowAnonymous();
    }

    public override async Task<FastEndpoints.Void> HandleAsync(Request request, CancellationToken ct)
    {
        var deleted = await handler.HandleAsync(request.Id, ct);

        if (!deleted)
            return await Send.NotFoundAsync(ct);

        return await Send.NoContentAsync(ct);
    }
}
