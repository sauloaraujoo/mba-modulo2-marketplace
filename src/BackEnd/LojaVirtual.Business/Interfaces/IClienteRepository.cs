using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteRepository : IDisposable
    {
        Task Insert(Cliente cliente, CancellationToken cancellationToken);
        Task<Cliente?> ObterClienteComFavoritos(Guid id, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
        void Update(Cliente cliente);
    }
}
