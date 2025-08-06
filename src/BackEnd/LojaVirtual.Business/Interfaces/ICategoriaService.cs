using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task Insert(Categoria request, CancellationToken tokenDeCancelamento);
        Task Edit(Categoria request, CancellationToken tokenDeCancelamento);
        Task Remove(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Categoria>> List(CancellationToken tokenDeCancelamento);        
        Task<Categoria> GetById(Guid id, CancellationToken tokenDeCancelamento);
    }
}
