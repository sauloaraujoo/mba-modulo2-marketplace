using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task Inserir(Categoria request, CancellationToken tokenDeCancelamento);
        Task Editar(Categoria request, CancellationToken tokenDeCancelamento);
        Task Remover(Guid id, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Categoria>> Listar(CancellationToken tokenDeCancelamento);        
        Task<Categoria> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
    }
}
