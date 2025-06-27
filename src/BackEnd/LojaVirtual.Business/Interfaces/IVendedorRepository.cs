using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorRepository : IDisposable
    {
        Task Insert(Vendedor request, CancellationToken cancellationToken);
        Task<Vendedor> GetById(Guid id, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
