using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LojaVirtual.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAppIdentifyUser _appIdentityUser;

        public ClienteService(
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository,
            IAppIdentifyUser appIdentityUser)
        {
            _clienteRepository = clienteRepository;
            _appIdentityUser = appIdentityUser;
        }

        public async Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetById(clienteId, cancellationToken);
            return cliente?.Favoritos ?? Enumerable.Empty<Favorito>();
        }

        public async Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetById(clienteId, cancellationToken);
            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            var jaExiste = cliente.Favoritos.Any(f => f.ProdutoId == produtoId);
            if (jaExiste)
            {
                return false;
            }

            cliente.AddFavorito(produtoId);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(cancellationToken);

            return true;
        }

        public async Task<bool> RemoverFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetById(clienteId, cancellationToken);

            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            var favorito = cliente.Favoritos.FirstOrDefault(f => f.ProdutoId == produtoId);
            if (favorito == null)
                return false;

            cliente.RemoveFavorito(favorito);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(cancellationToken);

            return true;
        }
    }

}
