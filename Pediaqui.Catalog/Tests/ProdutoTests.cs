using Application.Features.ProdutoContext;
using Application.Features.ProdutoContext.Create;
using Application.Notifications;
using Domain.Produto.Entities;
using Domain.Produto.Enuns;
using Domain.Produto.Extensions;
using Domain.Produto.Ports;
using Moq;

namespace Tests
{
    public class ProdutoTests
    {
        public ProdutoTests()
        {
        }

        [Fact]
        public async Task Handle_DeveRetornarProduto_QuandoProdutoCadastradoComSucesso()
        {
            // Arrange
            var mockProductRepository = new Mock<IProdutoRepository>();
            var mockProdutoPresenter = new Mock<IProdutoPresenter>();

            // Configurações do Produto
            var produto = new Produto("Teste", "Produto de teste", CategoriaProduto.LANCHE, 10m) { Id = 1 };
            var viewModel = new ProdutoResponse()
            {
                Id = 1,
                Nome = "Teste",
                Descricao = "Produto de teste",
                Categoria = CategoriaProduto.LANCHE.ToText(),
                Preco = 10m
            };

            mockProductRepository
                .Setup(repo => repo.Adicionar(It.IsAny<Produto>()))
                .Callback<Produto>(p => p.Id = produto.Id) // Simula a atribuição do Id
                .Returns(Task.FromResult<Produto>(produto));

            // Simula o comportamento do ProdutoPresenter
            mockProdutoPresenter
                .Setup(presenter => presenter.ToProdutoResponse(It.IsAny<Produto>()))
                .Returns(Task.FromResult<ProdutoResponse>(viewModel));

            var notificationContext = new NotificationContext();
            var handler = new CreateProdutoHandler(notificationContext, mockProductRepository.Object, mockProdutoPresenter.Object);
            var command = new CreateProdutoRequest()
            {
                Nome = "Teste",
                Descricao = "Produto de teste",
                Categoria = CategoriaProduto.LANCHE.ToInt(),
                Preco = 10m
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(viewModel.Nome, result.Nome);
            Assert.Equal(viewModel.Preco, result.Preco);

            mockProductRepository.Verify(repo => repo.Adicionar(It.IsAny<Produto>()), Times.Once);
            mockProdutoPresenter.Verify(presenter => presenter.ToProdutoResponse(It.IsAny<Produto>()), Times.Once);
        }
    }
}