using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoService
    {
        Task Inserir(Produto request, CancellationToken tokenDeCancelamento);
        Task Editar(Produto request, CancellationToken tokenDeCancelamento);
        Task Remover(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> Listar(CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ListarVitrine(Guid? categoriaId, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ListarVitrinePorVendedor(Guid? vendedorId, CancellationToken tokenDeCancelamento);

        Task<PagedResult<Produto>> ListarVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        Task<PagedResult<Produto>> ListarVitrinePorVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);

        Task<Produto> ListarVitrinePorId(Guid? produtoId, CancellationToken tokenDeCancelamento);

        Task<Produto> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoria(CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ObterTodosProdutosComCategoria(CancellationToken tokenDeCancelamento);
        Task<Produto> ObterProprioComCategoriaPorId(Guid id, CancellationToken tokenDeCancelamento);
        Task<Produto> ObterComCategoriaPorId(Guid id, CancellationToken tokenDeCancelamento);

        Task<Produto?> ObterProdutoProprioPorId(Guid id, CancellationToken tokenDeCancelamento);

        Task AlterarStatus(Produto request, CancellationToken tokenDeCancelamento);
    }
}
