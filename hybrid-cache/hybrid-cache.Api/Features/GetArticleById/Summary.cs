using FastEndpoints;
using hybrid_cache.Api.Domain;

namespace hybrid_cache.Api.Features.GetArticleById;

internal sealed class Summary : Summary<Endpoint, Request>
{
    public Summary()
    {
        ExampleRequest = new Request { Id = 1 };
        Summary = "Consulta um artigo pelo identificador";
        Description = "Retorna os dados de um artigo do blog a partir do seu Id. " +
            "O resultado é armazenado no HybridCache (memória local + Redis) com a chave 'article:{id}'.";
        Params["id"] = "Identificador do artigo";
        Response<Article>(200, "Artigo encontrado");
        Response(400, "O Id informado é inválido");
        Response(404, "Nenhum artigo foi encontrado com o Id informado");
    }
}
