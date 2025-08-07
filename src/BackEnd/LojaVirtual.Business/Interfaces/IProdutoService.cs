using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoService
    {
        Task Inserir(Produto request, CancellationToken cancellationToken);
        Task Editar(Produto request, CancellationToken cancellationToken);
        Task Remover(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> Listar(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ListarVitrine(Guid? categoriaId, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ListarVitrinePorVendedor(Guid? vendedorId, CancellationToken cancellationToken);

        Task<PagedResult<Produto>> ListarVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken cancellationToken);
        Task<PagedResult<Produto>> ListarVitrinePorVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken cancellationToken);

        Task<Produto> ListarVitrinePorId(Guid? produtoId, CancellationToken cancellationToken);

        Task<Produto> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoria(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterTodosProdutosComCategoria(CancellationToken cancellationToken);
        Task<Produto> ObterProprioComCategoriaPorId(Guid id, CancellationToken cancellationToken);
        Task<Produto> ObterComCategoriaPorId(Guid id, CancellationToken cancellationToken);

        Task<Produto?> ObterProdutoProprioPorId(Guid id, CancellationToken cancellationToken);

        Task AlterarStatus(Produto request, CancellationToken cancellationToken);
    }
}
