using Domain.Produto.Enuns;
using Domain.Produto.Ports;
using Microsoft.EntityFrameworkCore;
using Entities = Domain.Produto.Entities;

namespace Infra.Database.Repository.Produto;

public class ProdutoRepository : IProdutoRepository
{
    private DatabaseContext _context;

    public ProdutoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Entities.Produto> Adicionar(Entities.Produto produto)
    {
        var newProduto = await _context.Produtos.AddAsync(produto);
        _context.SaveChanges();

        return newProduto.Entity;
    }

    public async Task Atualizar(Entities.Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Deletar(int id)
    {
        var produto = await _context.Produtos.FirstAsync(p => p.Id == id);

        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<string> ObterNome(int id)
    {
        var p = await _context.Produtos.FirstOrDefaultAsync(s => s.Id == id);
        return p!.Nome;
    }

    public async Task<IEnumerable<Entities.Produto>> ObterPorCategoria(CategoriaProduto categoria)
    {
        return await _context.Produtos
            .Where(s => s.Categoria == categoria)
            .ToListAsync();
    }

    public async Task<Entities.Produto?> ObterPorId(int id)
    {
        return await _context.Produtos.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Entities.Produto>> ObterTodos()
    {
        return await _context.Produtos.ToListAsync();
    }
}
