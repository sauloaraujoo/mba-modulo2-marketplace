using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task Inserir(Categoria request, CancellationToken cancellationToken);
        Task Editar(Categoria request, CancellationToken cancellationToken);
        Task Remover(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Categoria>> Listar(CancellationToken cancellationToken);        
        Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken);
    }
}
