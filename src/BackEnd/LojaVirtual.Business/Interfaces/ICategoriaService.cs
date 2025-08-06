using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task Inserir(Categoria request, CancellationToken cancellationToken);
        Task Editar(Categoria request, CancellationToken cancellationToken);
        Task Remove(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Categoria>> List(CancellationToken cancellationToken);        
        Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken);
    }
}
