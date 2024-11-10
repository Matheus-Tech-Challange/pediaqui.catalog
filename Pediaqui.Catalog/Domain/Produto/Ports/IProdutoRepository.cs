using Domain.Produto.Enuns;

namespace Domain.Produto.Ports;

public interface IProdutoRepository
{
    Task<IEnumerable<Entities.Produto>> ObterTodos();
    Task<IEnumerable<Entities.Produto>> ObterPorCategoria(CategoriaProduto categoria);
    Task<Entities.Produto?> ObterPorId(int id);
    Task<string> ObterNome(int id);
    Task<Entities.Produto> Adicionar(Entities.Produto produto);
    Task Atualizar(Entities.Produto produto);
    Task Deletar(int id);
}
