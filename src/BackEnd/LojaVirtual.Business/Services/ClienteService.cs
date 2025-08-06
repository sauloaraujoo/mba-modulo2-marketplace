﻿using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAppIdentifyUser _appIdentityUser;
        private readonly INotificavel _notifiable;

        public ClienteService(
            IClienteRepository clienteRepository,
            IAppIdentifyUser appIdentityUser,
            INotificavel notifiable)
        {
            _clienteRepository = clienteRepository;
            _appIdentityUser = appIdentityUser;
            _notifiable = notifiable;
        }

        public async Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, cancellationToken);

            return cliente?.Favoritos ?? Enumerable.Empty<Favorito>();
        }

        public async Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, cancellationToken);

            if (cliente == null)
            {
                _notifiable.AdicionarNotificacao(new Notificacao("Cliente não encontrado."));
                return false;
            }

            var jaExiste = cliente.Favoritos.Any(f => f.ProdutoId == produtoId);
            if (jaExiste)
            {
                _notifiable.AdicionarNotificacao(new Notificacao("Produto já está nos favoritos."));
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
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, cancellationToken);

            if (cliente == null)
            {
                _notifiable.AdicionarNotificacao(new Notificacao("Cliente não encontrado."));
                return false;
            }

            var favorito = cliente.Favoritos.FirstOrDefault(f => f.ProdutoId == produtoId);
            if (favorito == null)
            {
                _notifiable.AdicionarNotificacao(new Notificacao("Produto não estava nos favoritos."));
                return false;
            }

            cliente.RemoveFavorito(favorito);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(cancellationToken);

            return true;
        }

        public async Task<PagedResult<Favorito>> GetFavoritosPaginado(int pagina, int tamanho, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, cancellationToken);

            return new PagedResult<Favorito>()
            {
                TotalItens = cliente.Favoritos.Count,
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                Itens = cliente.Favoritos.Skip((pagina - 1) * tamanho).Take(tamanho)
            };


            
        }
    }
}
