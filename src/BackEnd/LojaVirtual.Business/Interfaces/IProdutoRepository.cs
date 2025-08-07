using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        public Task Insert(Produto produto, CancellationToken tokenDeCancelamento);
        public Task<Produto> GetById(Guid id, CancellationToken tokenDeCancelamento);
        Task<Produto> GetSelfProdutoById(Guid id, Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> ListWithCategoriaVendedorAsNoTracking(CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorPagedAsNoTracking(int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorByCategoriaPagedAsNoTracking(Guid categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorByVendedorPagedAsNoTracking(Guid vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<Produto> getProdutoWithCategoriaVendedorById(Guid produtoId, CancellationToken tokenDeCancelamento);

        public Task<List<Produto>> ListWithCategoriaVendedorByCategoriaAsNoTracking(Guid categoriaId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> ListWithCategoriaVendedorByVendedorAsNoTracking(Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> List(Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task Edit(Produto produto, CancellationToken tokenDeCancelamento);
        public Task Remove(Produto produto, CancellationToken tokenDeCancelamento);
        public Task<bool> Exists(string nome, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(Guid vendedorid, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken tokenDeCancelamento);
        Task<Produto> GetSelfWithCategoriaById(Guid id, Guid vendedorid, CancellationToken tokenDeCancelamento);
        Task<Produto> GetWithCategoriaById(Guid id, CancellationToken tokenDeCancelamento);
        public Task<int> SaveChanges(CancellationToken tokenDeCancelamento);
    }
}
