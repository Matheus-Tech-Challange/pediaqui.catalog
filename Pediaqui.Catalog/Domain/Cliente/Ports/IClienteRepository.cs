using Domain.Common.Entities;

namespace Domain.Cliente.Ports;

public interface IClienteRepository
{
    Task<Entities.Cliente> CadastrarCliente(Entities.Cliente cliente);
    Task<Entities.Cliente?> BuscarPorCpf(string cpf);
    Task<Entities.Cliente?> BuscarPorId(int id);
    Task<string> BuscarNomePorId(int id);
}
