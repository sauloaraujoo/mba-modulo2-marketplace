using LojaVirtual.Core.Business.Entities;

namespace LojaVirtual.Core.Business.Interfaces
{
    public interface IVendedorService
    {
        Task Insert(Vendedor request, CancellationToken cancellationToken);
        Task Edit(Vendedor request, CancellationToken cancellationToken);
        Task Remove(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Vendedor>> List(CancellationToken cancellationToken);
        Task<Vendedor> GetById(Guid id, CancellationToken cancellationToken);
        Task<Vendedor?> GetSelfVendedorById(Guid id, CancellationToken cancellationToken);
    }
}
