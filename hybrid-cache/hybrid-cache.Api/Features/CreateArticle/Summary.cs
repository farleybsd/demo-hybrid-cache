using FastEndpoints;
using hybrid_cache.Api.Domain;

namespace hybrid_cache.Api.Features.CreateArticle;

internal sealed class Summary : Summary<Endpoint, ArticleInput>
{
    public Summary()
    {
        Summary = "Cadastra um novo artigo";
        Description = "Cria um artigo no blog e invalida o cache dos artigos (tag 'articles'), " +
            "para que as próximas consultas já tragam o artigo novo.";
        ExampleRequest = new ArticleInput
        {
            Title = "Introdução ao HybridCache",
            CategoryId = 1,
            Slug = "introducao-hybrid-cache",
            Content = "O HybridCache combina cache em memória com cache distribuído."
        };
        Response<Article>(201, "Artigo cadastrado com sucesso");
        Response(400, "Os dados informados são inválidos");
    }
}
