using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAppIdentifyUser _appIdentityUser;
        private readonly INotificavel _notificavel;

        public ClienteService(
            IClienteRepository clienteRepository,
            IAppIdentifyUser appIdentityUser,
            INotificavel notificavel)
        {
            _clienteRepository = clienteRepository;
            _appIdentityUser = appIdentityUser;
            _notificavel = notificavel;
        }

        public async Task<IEnumerable<Favorito>> GetFavoritos(CancellationToken tokenDeCancelamento)
        {
            var clienteId = Guid.Parse(_appIdentityUser.ObterUsuarioId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, tokenDeCancelamento);

            return cliente?.Favoritos ?? Enumerable.Empty<Favorito>();
        }

        public async Task<bool> AdicionarFavorito(Guid produtoId, CancellationToken tokenDeCancelamento)
        {
            var clienteId = Guid.Parse(_appIdentityUser.ObterUsuarioId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, tokenDeCancelamento);

            if (cliente == null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Cliente não encontrado."));
                return false;
            }

            var jaExiste = cliente.Favoritos.Any(f => f.ProdutoId == produtoId);
            if (jaExiste)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto já está nos favoritos."));
                return false;
            }

            cliente.AddFavorito(produtoId);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(tokenDeCancelamento);

            return true;
        }

        public async Task<bool> RemoverFavorito(Guid produtoId, CancellationToken tokenDeCancelamento)
        {
            var clienteId = Guid.Parse(_appIdentityUser.ObterUsuarioId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, tokenDeCancelamento);

            if (cliente == null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Cliente não encontrado."));
                return false;
            }

            var favorito = cliente.Favoritos.FirstOrDefault(f => f.ProdutoId == produtoId);
            if (favorito == null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não estava nos favoritos."));
                return false;
            }

            cliente.RemoveFavorito(favorito);
            _clienteRepository.Update(cliente);
            await _clienteRepository.SaveChanges(tokenDeCancelamento);

            return true;
        }

        public async Task<PagedResult<Favorito>> GetFavoritosPaginado(int pagina, int tamanho, CancellationToken tokenDeCancelamento)
        {
            var clienteId = Guid.Parse(_appIdentityUser.ObterUsuarioId());
            var cliente = await _clienteRepository.GetClienteComFavoritos(clienteId, tokenDeCancelamento);

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
