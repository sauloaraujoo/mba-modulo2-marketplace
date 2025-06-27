using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly IFavoritoRepository _favoritoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly INotifiable _notifiable;

        public FavoritoService(
            IFavoritoRepository favoritoRepository,
            IProdutoRepository produtoRepository,
            IClienteRepository clienteRepository,
            INotifiable notifiable)
        {
            _favoritoRepository = favoritoRepository;
            _produtoRepository = produtoRepository;
            _clienteRepository = clienteRepository;
            _notifiable = notifiable;
        }

        public async Task AdicionarFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetById(produtoId, cancellationToken);
            if (produto is null || !produto.Ativo)
            {
                _notifiable.AddNotification(new Notification("Produto não encontrado ou está inativo."));
                return;
            }

            var favoritoExistente = await _favoritoRepository.GetByClienteProduto(clienteId, produtoId, cancellationToken);
            if (favoritoExistente is not null)
            {
                _notifiable.AddNotification(new Notification("Produto já está nos favoritos."));
                return;
            }

            var favorito = new Favorito(clienteId, produtoId);
            await _favoritoRepository.Insert(favorito, cancellationToken);
            await _favoritoRepository.SaveChanges(cancellationToken);
        }

        public async Task RemoverFavorito(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
        {
            var favorito = await _favoritoRepository.GetByClienteProduto(clienteId, produtoId, cancellationToken);

            if (favorito is null)
            {
                _notifiable.AddNotification(new Notification("Produto não está na lista de favoritos."));
                return;
            }

            await _favoritoRepository.Remove(favorito, cancellationToken);
            await _favoritoRepository.SaveChanges(cancellationToken);
        }

        public async Task<IEnumerable<Produto>> ListarFavoritos(Guid clienteId, CancellationToken cancellationToken)
        {
            var favoritos = await _favoritoRepository.ListByCliente(clienteId, cancellationToken);

            return favoritos
                .Select(f => f.Produto)
                .Where(p => p.Ativo)
                .ToList();
        }
    }
}
