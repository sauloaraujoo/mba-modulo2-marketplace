using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorService
    {

        Task AlterarStatus(Vendedor request, CancellationToken cancellationToken);
        Task<IEnumerable<Vendedor>> List(CancellationToken cancellationToken);
        Task<Vendedor> GetById(Guid id, CancellationToken cancellationToken);
     
    }
}
