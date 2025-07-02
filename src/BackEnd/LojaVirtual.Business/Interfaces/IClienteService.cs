using LojaVirtual.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken);
        Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken cancellationToken);
        Task<bool> RemoverFavorito(Guid produtoId, CancellationToken cancellationToken);
    }
}
