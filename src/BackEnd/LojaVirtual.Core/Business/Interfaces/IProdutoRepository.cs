using LojaVirtual.Core.Business.Entities;

namespace LojaVirtual.Core.Business.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        public Task Insert(Produto produto, CancellationToken cancellationToken);
        public Task<Produto> GetById(Guid id, CancellationToken cancellationToken);
        Task<Produto> GetSelfProdutoById(Guid id, Guid vendedorId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListWithCategoriaVendedorAsNoTracking(CancellationToken cancellationToken);
        public Task<List<Produto>> ListWithCategoriaVendedorByCategoriaAsNoTracking(Guid categoriaId, CancellationToken cancellationToken);        
        public Task<List<Produto>> List(Guid vendedorId, CancellationToken cancellationToken);
        public Task Edit(Produto produto, CancellationToken cancellationToken);
        public Task Remove(Produto produto, CancellationToken cancellationToken);
        public Task<bool> Exists(string nome, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(Guid vendedorid, CancellationToken cancellationToken);
        Task<Produto> GetSelfWithCategoriaById(Guid id, Guid vendedorid, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
