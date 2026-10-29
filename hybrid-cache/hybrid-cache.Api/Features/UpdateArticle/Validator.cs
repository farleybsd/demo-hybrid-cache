using FastEndpoints;
using FluentValidation;

namespace hybrid_cache.Api.Features.UpdateArticle;

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Id)
            .GreaterThan(0)
            .WithMessage("O Id do artigo deve ser maior que zero.");

        RuleFor(r => r.Title)
            .NotEmpty()
            .WithMessage("O título do artigo é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O título do artigo deve ter no máximo 200 caracteres.");

        RuleFor(r => r.Slug)
            .NotEmpty()
            .WithMessage("O slug do artigo é obrigatório.");

        RuleFor(r => r.Content)
            .NotEmpty()
            .WithMessage("O conteúdo do artigo é obrigatório.");

        RuleFor(r => r.CategoryId)
            .GreaterThan(0)
            .WithMessage("A categoria do artigo é obrigatória.");
    }
}
