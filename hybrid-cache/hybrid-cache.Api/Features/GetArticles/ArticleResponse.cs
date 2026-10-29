namespace hybrid_cache.Api.Features.GetArticles
{
    public class ArticleResponse
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
