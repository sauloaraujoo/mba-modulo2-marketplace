using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly INotificavel _notificavel;
        private readonly IAppIdentifyUser _appIdentifyUser;
        public ProdutoService(
            ICategoriaRepository categoriaRepository,
            IProdutoRepository produtoRepository,
            INotificavel notificavel,
            IAppIdentifyUser appIdentifyUser)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
            _notificavel = notificavel;
            _appIdentifyUser = appIdentifyUser;
        }
        public async Task Insert(Produto request, CancellationToken tokenDeCancelamento)
        {
            //verifica se a categoria existe
            if (await _categoriaRepository.GetById(request.CategoriaId, tokenDeCancelamento) is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não existente."));
            }
            
            request.VinculaVendedor(new Guid(_appIdentifyUser.GetUserId()));
            await _produtoRepository.Insert(request, tokenDeCancelamento);
            await _produtoRepository.SaveChanges(tokenDeCancelamento);
        }
        public async Task Remove(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = await GetSelfProdutoById(id, tokenDeCancelamento);
            if (produto is null) { return; }

            await _produtoRepository.Remove(produto!, tokenDeCancelamento);
            await _produtoRepository.SaveChanges(tokenDeCancelamento);
        }
        public async Task Edit(Produto request, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.GetById(request.CategoriaId, tokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            
            var produto = await GetSelfProdutoById(request.Id, tokenDeCancelamento);
            if (produto is null) { return; }

            produto.Edit(request.Nome, request.Descricao, request.Imagem, request.Preco, request.Estoque, true, request.CategoriaId);

            await _produtoRepository.Edit(produto, tokenDeCancelamento);
            await _produtoRepository.SaveChanges(tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.GetAllSelfProdutoWithCategoria(new Guid(_appIdentifyUser.GetUserId()), tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.GetAllProdutoWithCategoria(tokenDeCancelamento);
        }

        public async Task<Produto> GetSelfWithCategoriaById(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.GetSelfWithCategoriaById(id, new Guid(_appIdentifyUser.GetUserId()), tokenDeCancelamento);
        }
        public async Task<Produto> GetWithCategoriaById(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.GetWithCategoriaById(id, tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> List(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.List(new Guid(_appIdentifyUser.GetUserId()), tokenDeCancelamento);
        }
        public async Task<Produto?> GetSelfProdutoById(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = await _produtoRepository.GetSelfProdutoById(id, new Guid(_appIdentifyUser.GetUserId()), tokenDeCancelamento);
            if (produto is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return null;
            }
            return produto;
        }

        public async Task<IEnumerable<Produto>> ListVitrine(Guid? categoriaId, CancellationToken tokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
                await _produtoRepository.ListWithCategoriaVendedorAsNoTracking(tokenDeCancelamento) :
                await _produtoRepository.ListWithCategoriaVendedorByCategoriaAsNoTracking(new Guid(categoriaId.ToString()!), tokenDeCancelamento);
            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListVitrineByVendedor(Guid? vendedorId, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ListWithCategoriaVendedorByVendedorAsNoTracking(new Guid(vendedorId.ToString()!), tokenDeCancelamento);
        }

        public async Task<Produto> ListVitrineById(Guid? produtoId, CancellationToken tokenDeCancelamento)
        {
            var produto = await _produtoRepository.getProdutoWithCategoriaVendedorById(new Guid(produtoId.ToString()!), tokenDeCancelamento);
            return produto;
        }

        public async Task<Produto> GetById(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.GetById(id, tokenDeCancelamento);
        }
        public async Task AlterarStatus(Produto produto, CancellationToken tokenDeCancelamento)
        {
            var produtoOrigem = await _produtoRepository.GetById(produto.Id, tokenDeCancelamento);
            if (produtoOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return;
            }

            produtoOrigem.AlterarStatus();

            await _produtoRepository.Edit(produtoOrigem, tokenDeCancelamento);
            await _produtoRepository.SaveChanges(tokenDeCancelamento);
        }

        public async Task<PagedResult<Produto>> ListVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
               await _produtoRepository.ListWithCategoriaVendedorPagedAsNoTracking(pagina, tamanho, tokenDeCancelamento) :
               await _produtoRepository.ListWithCategoriaVendedorByCategoriaPagedAsNoTracking(new Guid(categoriaId.ToString()!), pagina, tamanho, tokenDeCancelamento);
            return produtos;
        }

        public async Task<PagedResult<Produto>> ListVitrineByVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ListWithCategoriaVendedorByVendedorPagedAsNoTracking(new Guid(vendedorId.ToString()!), pagina, tamanho, tokenDeCancelamento);
        }
    }
}
