using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteRepository : IDisposable
    {
        Task Insert(Cliente cliente, CancellationToken tokenDeCancelamento);
        Task<Cliente?> ObterClienteComFavoritos(Guid id, CancellationToken tokenDeCancelamento);
        public Task<int> SaveChanges(CancellationToken tokenDeCancelamento);
        void Update(Cliente cliente);
    }
}
