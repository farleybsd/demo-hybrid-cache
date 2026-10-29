using FastEndpoints;
using FluentValidation;

namespace hybrid_cache.Api.Features.GetArticleById;

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O Id do artigo deve ser maior que zero.");
    }
}
