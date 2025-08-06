using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAppIdentifyUser _appIdentityUser;
        private readonly INotifiable _notifiable;

        public ClienteService(
            IClienteRepository clienteRepository,
            IAppIdentifyUser appIdentityUser,
            INotifiable notifiable)
        {
            _clienteRepository = clienteRepository;
            _appIdentityUser = appIdentityUser;
            _notifiable = notifiable;
        }

        public async Task<IEnumerable<Favorito>> ObterFavoritos(CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.ObterClienteComFavoritos(clienteId, cancellationToken);

            return cliente?.Favoritos ?? Enumerable.Empty<Favorito>();
        }

        public async Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.ObterClienteComFavoritos(clienteId, cancellationToken);

            if (cliente == null)
            {
                _notifiable.AddNotification(new Notification("Cliente não encontrado."));
                return false;
            }

            var jaExiste = cliente.Favoritos.Any(f => f.ProdutoId == produtoId);
            if (jaExiste)
            {
                _notifiable.AddNotification(new Notification("Produto já está nos favoritos."));
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
            var cliente = await _clienteRepository.ObterClienteComFavoritos(clienteId, cancellationToken);

            if (cliente == null)
            {
                _notifiable.AddNotification(new Notification("Cliente não encontrado."));
                return false;
            }

            var favorito = cliente.Favoritos.FirstOrDefault(f => f.ProdutoId == produtoId);
            if (favorito == null)
            {
                _notifiable.AddNotification(new Notification("Produto não estava nos favoritos."));
                return false;
            }

            cliente.RemoveFavorito(favorito);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(cancellationToken);

            return true;
        }

        public async Task<PagedResult<Favorito>> ObterFavoritosPaginado(int pagina, int tamanho, CancellationToken cancellationToken)
        {
            var clienteId = Guid.Parse(_appIdentityUser.GetUserId());
            var cliente = await _clienteRepository.ObterClienteComFavoritos(clienteId, cancellationToken);

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
