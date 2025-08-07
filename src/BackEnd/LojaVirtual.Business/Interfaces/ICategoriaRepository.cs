using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaRepository : IDisposable
    {
        public Task Insert(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task<Categoria> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
        public Task<IList<Categoria>> ListAsNoTracking(CancellationToken tokenDeCancelamento);
        public Task Edit(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task Remove(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task<Categoria> ObterComProduto(Guid id, CancellationToken tokenDeCancelamento);
        public Task<bool> Exists(string nome, CancellationToken tokenDeCancelamento);
        public Task<int> SaveChanges(CancellationToken tokenDeCancelamento);
    }
}
