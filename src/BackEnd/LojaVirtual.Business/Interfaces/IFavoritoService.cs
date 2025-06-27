using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IFavoritoService
    {
        Task AdicionarFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);
        Task RemoverFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ListarFavoritos(Guid clienteId, CancellationToken cancellationToken);
    }
}