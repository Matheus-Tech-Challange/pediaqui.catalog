using Domain.Common.Ports;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Cliente.Validators.FluentValidator;

public class ClienteFluentValidator : IValidatorEntity<Entities.Cliente>
{
    public void Validate(Entities.Cliente entity)
    {
        var v = new ClienteValidation().Validate(entity);

        foreach (var error in v.Errors)
        {
            entity.AddError(error.ErrorCode, error.ErrorMessage);
        }
    }

    private class ClienteValidation : AbstractValidator<Entities.Cliente>
    {
        public ClienteValidation()
        {
            RuleFor(e => e.Cpf)
                .Matches(new Regex("^[0-9]{3}[0-9]{3}[0-9]{3}[0-9]{2}"))
                .WithMessage("CPF inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(5)
                .WithMessage("Nome deve ter no mínimo 5 caracteres");

            RuleFor(e => e.Email)
                .EmailAddress()
                .WithMessage("Email inválido");
        }
    }
}
