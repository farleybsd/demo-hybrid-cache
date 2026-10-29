namespace hybrid_cache.Api.Features.UpdateArticle;

public sealed class Request
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}
