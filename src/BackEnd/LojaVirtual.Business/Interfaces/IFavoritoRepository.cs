using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IFavoritoRepository : IDisposable
    {
        Task Insert(Favorito favorito, CancellationToken cancellationToken);
        Task Remove(Favorito favorito, CancellationToken cancellationToken);
        Task<Favorito?> GetByClienteProduto(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);
        Task<IEnumerable<Favorito>> ListByCliente(Guid clienteId, CancellationToken cancellationToken);
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
