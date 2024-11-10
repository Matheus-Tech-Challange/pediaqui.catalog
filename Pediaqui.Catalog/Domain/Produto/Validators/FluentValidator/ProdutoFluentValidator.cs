using Domain.Common.Ports;
using FluentValidation;

namespace Domain.Produto.Validators.FluentValidator;

public class ProdutoFluentValidator : IValidatorEntity<Entities.Produto>
{
    public void Validate(Entities.Produto entity)
    {
        var v = new ProdutoValidator().Validate(entity);

        foreach (var error in v.Errors)
        {
            entity.AddError(error.ErrorCode, error.ErrorMessage);
        }
    }

    private class ProdutoValidator : AbstractValidator<Entities.Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Nome do produto deve ter tamanho mínimo de 3 caracteres");

            RuleFor(p => p.Nome)
                .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(p => p.Preco)
                .GreaterThan(0)
                .WithMessage("Preço deve ser maior que zero");

            RuleFor(p => p.Descricao)
                .MaximumLength(255)
                .WithMessage("Descrição deve ter no máximo 255 caracteres");

            RuleFor(p => p.Categoria)
                .IsInEnum()
                .WithMessage("Categoria inválida ou inexistente");
        }
    }
}
