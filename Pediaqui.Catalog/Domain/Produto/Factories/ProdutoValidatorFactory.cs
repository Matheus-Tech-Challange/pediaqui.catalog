using Domain.Common.Ports;
using Domain.Produto.Validators.FluentValidator;

namespace Domain.Produto.Factories;

public static class ProdutoValidatorFactory
{
    public static IValidatorEntity<Entities.Produto> Create()
    {
        return new ProdutoFluentValidator();
    }
}
