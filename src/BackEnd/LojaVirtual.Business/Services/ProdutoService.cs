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
        public async Task Inserir(Produto request, CancellationToken TokenDeCancelamento)
        {
            //verifica se a categoria existe
            if (await _categoriaRepository.ObterPorId(request.CategoriaId, TokenDeCancelamento) is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não existente."));
            }
            
            request.VinculaVendedor(new Guid(_appIdentifyUser.ObterUsuarioId()));
            await _produtoRepository.Inserir(request, TokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(TokenDeCancelamento);
        }
        public async Task Remover(Guid id, CancellationToken TokenDeCancelamento)
        {
            var produto = await ObterProdutoProprioPorId(id, TokenDeCancelamento);
            if (produto is null) { return; }

            await _produtoRepository.Remover(produto!, TokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(TokenDeCancelamento);
        }
        public async Task Editar(Produto request, CancellationToken TokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.ObterPorId(request.CategoriaId, TokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            
            var produto = await ObterProdutoProprioPorId(request.Id, TokenDeCancelamento);
            if (produto is null) { return; }

            produto.Edit(request.Nome, request.Descricao, request.Imagem, request.Preco, request.Estoque, true, request.CategoriaId);

            await _produtoRepository.Editar(produto, TokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(TokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoria(CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ObterTodosProdutosPropriosComCategoria(new Guid(_appIdentifyUser.ObterUsuarioId()), TokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosComCategoria(CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ObterTodosProdutosComCategoria(TokenDeCancelamento);
        }

        public async Task<Produto> ObterProprioComCategoriaPorId(Guid id, CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ObterProprioComCategoriaPorId(id, new Guid(_appIdentifyUser.ObterUsuarioId()), TokenDeCancelamento);
        }
        public async Task<Produto> ObterComCategoriaPorId(Guid id, CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ObterComCategoriaPorId(id, TokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> Listar(CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.Listar(new Guid(_appIdentifyUser.ObterUsuarioId()), TokenDeCancelamento);
        }
        public async Task<Produto?> ObterProdutoProprioPorId(Guid id, CancellationToken TokenDeCancelamento)
        {
            var produto = await _produtoRepository.ObterProdutoProprioPorId(id, new Guid(_appIdentifyUser.ObterUsuarioId()), TokenDeCancelamento);
            if (produto is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return null;
            }
            return produto;
        }

        public async Task<IEnumerable<Produto>> ListarVitrine(Guid? categoriaId, CancellationToken TokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
                await _produtoRepository.ListarComCategoriaVendedorSemContexto(TokenDeCancelamento) :
                await _produtoRepository.ListarComCategoriaVendedorPorCategoriaSemContexto(new Guid(categoriaId.ToString()!), TokenDeCancelamento);
            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListarVitrinePorVendedor(Guid? vendedorId, CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ListarComCategoriaVendedorPorVendedorSemContexto(new Guid(vendedorId.ToString()!), TokenDeCancelamento);
        }

        public async Task<Produto> ListarVitrinePorId(Guid? produtoId, CancellationToken TokenDeCancelamento)
        {
            var produto = await _produtoRepository.ObterProdutoComCategoriaVendedorPorId(new Guid(produtoId.ToString()!), TokenDeCancelamento);
            return produto;
        }

        public async Task<Produto> ObterPorId(Guid id, CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ObterPorId(id, TokenDeCancelamento);
        }
        public async Task AlterarStatus(Produto produto, CancellationToken TokenDeCancelamento)
        {
            var produtoOrigem = await _produtoRepository.ObterPorId(produto.Id, TokenDeCancelamento);
            if (produtoOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return;
            }

            produtoOrigem.AlterarStatus();

            await _produtoRepository.Editar(produtoOrigem, TokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(TokenDeCancelamento);
        }

        public async Task<PagedResult<Produto>> ListarVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken TokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
               await _produtoRepository.ListarComCategoriaVendedorPaginadoSemContexto(pagina, tamanho, TokenDeCancelamento) :
               await _produtoRepository.ListarComCategoriaVendedorPorCategoriaPaginadoSemContexto(new Guid(categoriaId.ToString()!), pagina, tamanho, TokenDeCancelamento);
            return produtos;
        }

        public async Task<PagedResult<Produto>> ListarVitrinePorVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken TokenDeCancelamento)
        {
            return await _produtoRepository.ListarComCategoriaVendedorPorVendedorPaginadoSemContexto(new Guid(vendedorId.ToString()!), pagina, tamanho, TokenDeCancelamento);
        }
    }
}
