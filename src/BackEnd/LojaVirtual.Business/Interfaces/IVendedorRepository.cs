using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorRepository : IDisposable
    {
        Task Insert(Vendedor request, CancellationToken tokenDeCancelamento);
        Task<Vendedor> GetById(Guid id, CancellationToken tokenDeCancelamento);
        public Task<IList<Vendedor>> ListAsNoTracking(CancellationToken tokenDeCancelamento);
        public Task Edit(Vendedor vendedor, CancellationToken tokenDeCancelamento);
        public Task<bool> Exists(string nome, CancellationToken tokenDeCancelamento);
        public Task<int> SaveChanges(CancellationToken tokenDeCancelamento);
    }
}
