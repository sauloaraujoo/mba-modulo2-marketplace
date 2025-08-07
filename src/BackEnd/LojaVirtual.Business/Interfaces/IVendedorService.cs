using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorService
    {

        Task AlterarStatus(Vendedor request, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Vendedor>> Listar(CancellationToken tokenDeCancelamento);
        Task<Vendedor> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
     
    }
}
