using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken);
        Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken cancellationToken);
        Task<PagedResult<Favorito>> GetFavoritosPaginado(int pagina, int tamanho, CancellationToken cancellationToken);
        Task<bool> RemoverFavorito(Guid produtoId, CancellationToken cancellationToken);
    }
}
