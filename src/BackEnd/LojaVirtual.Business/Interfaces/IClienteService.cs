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
        Task<IEnumerable<Favorito>> GetFavoritos(Guid clienteId, CancellationToken cancellationToken);
        Task<bool> AdicionarFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);
        Task RemoverFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);
    }
}
