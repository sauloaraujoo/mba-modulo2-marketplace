using LojaVirtual.Core.Business.Entities;

namespace LojaVirtual.Core.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task Insert(Categoria request, CancellationToken cancellationToken);
        Task Edit(Categoria request, CancellationToken cancellationToken);
        Task Remove(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Categoria>> List(CancellationToken cancellationToken);        
        Task<Categoria> GetById(Guid id, CancellationToken cancellationToken);
    }
}
