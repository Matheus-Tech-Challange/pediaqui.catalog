using Domain.Cliente.Validators.FluentValidator;
using Domain.Common.Ports;

namespace Domain.Cliente.Factories;

public static class ClienteValidationFactory
{
    public static IValidatorEntity<Entities.Cliente> Create()
    {
        return new ClienteFluentValidator();
    }
}
