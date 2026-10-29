using FastEndpoints;

namespace hybrid_cache.Api.Features.GetArticles;

internal sealed class Summary : Summary<Endpoint>
{
    public Summary()
    {
        Summary = "Lista todos os artigos cadastrados";
        Description = "Retorna a coleção completa de artigos do blog, sem paginação. " +
            "O resultado é armazenado no HybridCache (memória local + Redis) com a chave 'articles'.";
        Response<IReadOnlyList<ArticleResponse>>(200, "Coleção de artigos");
    }
}
