using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly INotifiable _notifiable;
        private readonly IAppIdentifyUser _appIdentifyUser;
        public ProdutoService(
            ICategoriaRepository categoriaRepository,
            IProdutoRepository produtoRepository,
            INotifiable notifiable,
            IAppIdentifyUser appIdentifyUser)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
            _notifiable = notifiable;
            _appIdentifyUser = appIdentifyUser;
        }
        public async Task Insert(Produto request, CancellationToken cancellationToken)
        {
            //verifica se a categoria existe
            if (await _categoriaRepository.GetById(request.CategoriaId, cancellationToken) is null)
            {
                _notifiable.AddNotification(new Notification("Categoria não existente."));
            }
            
            request.VinculaVendedor(new Guid(_appIdentifyUser.GetUserId()));
            await _produtoRepository.Insert(request, cancellationToken);
            await _produtoRepository.SaveChanges(cancellationToken);
        }
        public async Task Remove(Guid id, CancellationToken cancellationToken)
        {
            var produto = await GetSelfProdutoById(id, cancellationToken);
            if (produto is null) { return; }

            await _produtoRepository.Remove(produto!, cancellationToken);
            await _produtoRepository.SaveChanges(cancellationToken);
        }
        public async Task Edit(Produto request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetById(request.CategoriaId, cancellationToken);
            if (categoria is null)
            {
                _notifiable.AddNotification(new Notification("Categoria não encontrada."));
                return;
            }
            
            var produto = await GetSelfProdutoById(request.Id, cancellationToken);
            if (produto is null) { return; }

            produto.Edit(request.Nome, request.Descricao, request.Imagem, request.Preco, request.Estoque, true, request.CategoriaId);

            await _produtoRepository.Edit(produto, cancellationToken);
            await _produtoRepository.SaveChanges(cancellationToken);
        }

        public async Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(CancellationToken cancellationToken)
        {
            return await _produtoRepository.GetAllSelfProdutoWithCategoria(new Guid(_appIdentifyUser.GetUserId()), cancellationToken);
        }

        public async Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken cancellationToken)
        {
            return await _produtoRepository.GetAllProdutoWithCategoria(cancellationToken);
        }

        public async Task<Produto> GetSelfWithCategoriaById(Guid id, CancellationToken cancellationToken)
        {
            return await _produtoRepository.GetSelfWithCategoriaById(id, new Guid(_appIdentifyUser.GetUserId()), cancellationToken);
        }
        public async Task<Produto> GetWithCategoriaById(Guid id, CancellationToken cancellationToken)
        {
            return await _produtoRepository.GetWithCategoriaById(id, cancellationToken);
        }

        public async Task<IEnumerable<Produto>> List(CancellationToken cancellationToken)
        {
            return await _produtoRepository.List(new Guid(_appIdentifyUser.GetUserId()), cancellationToken);
        }
        public async Task<Produto?> GetSelfProdutoById(Guid id, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetSelfProdutoById(id, new Guid(_appIdentifyUser.GetUserId()), cancellationToken);
            if (produto is null)
            {
                _notifiable.AddNotification(new Notification("Produto não encontrado."));
                return null;
            }
            return produto;
        }

        public async Task<IEnumerable<Produto>> ListVitrine(Guid? categoriaId, CancellationToken cancellationToken)
        {
            var produtos = categoriaId == null ?
                await _produtoRepository.ListWithCategoriaVendedorAsNoTracking(cancellationToken) :
                await _produtoRepository.ListWithCategoriaVendedorByCategoriaAsNoTracking(new Guid(categoriaId.ToString()!), cancellationToken);
            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListVitrineByVendedor(Guid? vendedorId, CancellationToken cancellationToken)
        {
            return await _produtoRepository.ListWithCategoriaVendedorByVendedorAsNoTracking(new Guid(vendedorId.ToString()!), cancellationToken);
        }

        public async Task<Produto> ListVitrineById(Guid? produtoId, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.getProdutoWithCategoriaVendedorById(new Guid(produtoId.ToString()!), cancellationToken);
            return produto;
        }

        public async Task<Produto> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _produtoRepository.GetById(id, cancellationToken);
        }
        public async Task AlterarStatus(Produto produto, CancellationToken cancellationToken)
        {
            var produtoOrigem = await _produtoRepository.GetById(produto.Id, cancellationToken);
            if (produtoOrigem is null)
            {
                _notifiable.AddNotification(new Notification("Produto não encontrado."));
                return;
            }

            produtoOrigem.AlterarStatus();

            await _produtoRepository.Edit(produtoOrigem, cancellationToken);
            await _produtoRepository.SaveChanges(cancellationToken);
        }
    }
}
