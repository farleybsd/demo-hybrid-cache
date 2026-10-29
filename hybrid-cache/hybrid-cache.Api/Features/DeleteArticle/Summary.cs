using FastEndpoints;

namespace hybrid_cache.Api.Features.DeleteArticle;

internal sealed class Summary : Summary<Endpoint, Request>
{
    public Summary()
    {
        Summary = "Exclui um artigo pelo identificador";
        Description = "Remove um artigo do blog a partir do seu Id " +
            "e invalida o cache dos artigos (tag 'articles').";
        Params["id"] = "Identificador do artigo";
        ExampleRequest = new Request { Id = 1 };
        Response(204, "Artigo excluído com sucesso");
        Response(400, "O Id informado é inválido");
        Response(404, "Nenhum artigo foi encontrado com o Id informado");
    }
}
