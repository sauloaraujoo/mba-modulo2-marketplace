using LojaVirtual.Core.Business.Entities;

namespace LojaVirtual.Core.Business.Interfaces
{
    public interface ICategoriaRepository : IDisposable
    {
        public Task Insert(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> GetById(Guid id, CancellationToken cancellationToken);
        public Task<IList<Categoria>> ListAsNoTracking(CancellationToken cancellationToken);
        public Task Edit(Categoria categoria, CancellationToken cancellationToken);
        public Task Remove(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> GetWithProduto(Guid id, CancellationToken cancellationToken);
        public Task<bool> Exists(string nome, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
