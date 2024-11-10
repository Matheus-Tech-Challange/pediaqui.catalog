using Domain.Produto.Ports;

namespace Application.Features.ProdutoContext.Delete;

public class DeleteProdutoHandler : IRequestHandler<DeleteProdutoRequest>
{
    private readonly NotificationContext _notificationContext;
    private readonly IProdutoRepository _produtoRepository;

    public DeleteProdutoHandler(NotificationContext notificationContext, IProdutoRepository produtoRepository)
    {
        _notificationContext = notificationContext;
        _produtoRepository = produtoRepository;
    }

    public async Task Handle(DeleteProdutoRequest request, CancellationToken cancellationToken)
    {
        await _produtoRepository.Deletar(request.Id);
    }
}
