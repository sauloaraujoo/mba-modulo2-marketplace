using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorService
    {

        Task AlterarStatus(Vendedor request, CancellationToken cancellationToken);
        Task<IEnumerable<Vendedor>> Listar(CancellationToken cancellationToken);
        Task<Vendedor> ObterPorId(Guid id, CancellationToken cancellationToken);
     
    }
}
