using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteRepository : IDisposable
    {
        Task Insert(Cliente cliente, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
