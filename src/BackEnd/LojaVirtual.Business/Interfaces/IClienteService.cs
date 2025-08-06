using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken tokenDeCancelamento);
        Task<PagedResult<Favorito>> GetFavoritosPaginado(int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        Task<bool> RemoverFavorito(Guid produtoId, CancellationToken tokenDeCancelamento);
    }
}
