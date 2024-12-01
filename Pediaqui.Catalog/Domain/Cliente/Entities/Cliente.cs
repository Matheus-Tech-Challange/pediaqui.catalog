using Domain.Cliente.Factories;
using Domain.Common.Entities;

namespace Domain.Cliente.Entities;

public class Cliente : Entity
{
    public Cliente(string nome, string email, string cpf)
    {
        Nome = nome;
        Email = email;
        Cpf = cpf;

        Validar<Cliente>(this, ClienteValidationFactory.Create());
    }

    public string Nome { get; }
    public string Email { get; }
    public string Cpf { get; }
}
