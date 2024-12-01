using Microsoft.EntityFrameworkCore;
using Entities = Domain.Cliente.Entities;
using Ports = Domain.Cliente.Ports;

namespace Infra.Database.Repository.Cliente;

public class ClienteRepository : Ports.IClienteRepository
{
    private DatabaseContext _context;

    public ClienteRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> BuscarNomePorId(int id)
    {
        var c = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        return c!.Nome;
    }

    public async Task<Entities.Cliente?> BuscarPorCpf(string cpf)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<Entities.Cliente?> BuscarPorId(int id)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Entities.Cliente> CadastrarCliente(Entities.Cliente cliente)
    {
        var entity = await _context.Clientes.AddAsync(cliente);
        _context.SaveChanges();

        return entity.Entity;
    }
}
