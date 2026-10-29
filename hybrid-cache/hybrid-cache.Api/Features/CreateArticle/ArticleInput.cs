namespace hybrid_cache.Api.Features.CreateArticle;

public class ArticleInput
{
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
}
