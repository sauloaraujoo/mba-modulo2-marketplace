using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoService
    {
        Task Insert(Produto request, CancellationToken tokenDeCancelamento);
        Task Edit(Produto request, CancellationToken tokenDeCancelamento);
        Task Remove(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> List(CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ListVitrine(Guid? categoriaId, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ListVitrineByVendedor(Guid? vendedorId, CancellationToken tokenDeCancelamento);

        Task<PagedResult<Produto>> ListVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        Task<PagedResult<Produto>> ListVitrineByVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);

        Task<Produto> ListVitrineById(Guid? produtoId, CancellationToken tokenDeCancelamento);

        Task<Produto> GetById(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken tokenDeCancelamento);
        Task<Produto> GetSelfWithCategoriaById(Guid id, CancellationToken tokenDeCancelamento);
        Task<Produto> GetWithCategoriaById(Guid id, CancellationToken tokenDeCancelamento);

        Task<Produto?> GetSelfProdutoById(Guid id, CancellationToken tokenDeCancelamento);

        Task AlterarStatus(Produto request, CancellationToken tokenDeCancelamento);
    }
}
