using FastEndpoints;

namespace hybrid_cache.Api.Features.UpdateArticle;

internal sealed class Summary : Summary<Endpoint, Request>
{
    public Summary()
    {
        Summary = "Atualiza um artigo existente";
        Description = "Altera título, slug, conteúdo e categoria de um artigo a partir do seu Id " +
            "e invalida o cache dos artigos (tag 'articles').";
        Params["id"] = "Identificador do artigo";
        ExampleRequest = new Request
        {
            Id = 1,
            CategoryId = 1,
            Title = "Introdução ao HybridCache (revisado)",
            Slug = "introducao-hybrid-cache",
            Content = "O HybridCache combina cache em memória com cache distribuído."
        };
        Response(204, "Artigo atualizado com sucesso");
        Response(400, "Os dados informados são inválidos");
        Response(404, "Nenhum artigo foi encontrado com o Id informado");
    }
}
