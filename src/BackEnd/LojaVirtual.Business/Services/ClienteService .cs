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
        private readonly IProdutoRepository _produtoRepository;

        public ClienteService(IClienteRepository clienteRepository, IProdutoRepository produtoRepository)
        {
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Favorito>> GetFavoritos(Guid clienteId, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.GetById(clienteId, cancellationToken);
            return cliente?.Favoritos ?? Enumerable.Empty<Favorito>();
        }

        public async Task<bool> AdicionarFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
        {
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

        public async Task RemoverFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.GetById(clienteId, cancellationToken);
            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            var favorito = cliente.Favoritos.FirstOrDefault(f => f.ProdutoId == produtoId);
            if (favorito != null)
            {
                cliente.RemoveFavorito(favorito);
                _clienteRepository.Update(cliente);
                await _clienteRepository.SaveChanges(cancellationToken);
            }
        }
    }
}
