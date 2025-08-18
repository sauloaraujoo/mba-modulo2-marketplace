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
        public async Task Inserir(Produto request, CancellationToken tokenDeCancelamento)
        {
            //verifica se a categoria existe
            if (await _categoriaRepository.ObterPorId(request.CategoriaId, tokenDeCancelamento) is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não existente."));
            }
            
            request.VinculaVendedor(new Guid(_appIdentifyUser.ObterUsuarioId()));
            await _produtoRepository.Inserir(request, tokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(tokenDeCancelamento);
        }
        public async Task Remover(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = await ObterProdutoProprioPorId(id, tokenDeCancelamento);
            if (produto is null) { return; }

            await _produtoRepository.Remover(produto!, tokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(tokenDeCancelamento);
        }
        public async Task Editar(Produto request, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.ObterPorId(request.CategoriaId, tokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            
            var produto = await ObterProdutoProprioPorId(request.Id, tokenDeCancelamento);
            if (produto is null) { return; }

            produto.Editar(request.Nome, request.Descricao, request.Imagem, request.Preco, request.Estoque, true, request.CategoriaId);

            await _produtoRepository.Editar(produto, tokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoriaVendedor(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ObterTodosProdutosPropriosComCategoriaVendedor(new Guid(_appIdentifyUser.ObterUsuarioId()), tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> ObterTodosProdutosComCategoriaVendedor(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ObterTodosProdutosComCategoriaVendedor(tokenDeCancelamento);
        }

        public async Task<Produto> ObterProprioComCategoriaVendedorPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ObterProprioComCategoriaVendedorPorId(id, new Guid(_appIdentifyUser.ObterUsuarioId()), tokenDeCancelamento);
        }
        public async Task<Produto> ObterComCategoriaVendedorPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ObterComCategoriaVendedorPorId(id, tokenDeCancelamento);
        }

        public async Task<IEnumerable<Produto>> Listar(CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.Listar(new Guid(_appIdentifyUser.ObterUsuarioId()), tokenDeCancelamento);
        }
        public async Task<Produto?> ObterProdutoProprioPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = await _produtoRepository.ObterProdutoProprioPorId(id, new Guid(_appIdentifyUser.ObterUsuarioId()), tokenDeCancelamento);
            if (produto is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return null;
            }
            return produto;
        }

        public async Task<IEnumerable<Produto>> ListarVitrine(Guid? categoriaId, CancellationToken tokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
                await _produtoRepository.ListarComCategoriaVendedorSemContexto(tokenDeCancelamento) :
                await _produtoRepository.ListarComCategoriaVendedorPorCategoriaSemContexto(new Guid(categoriaId.ToString()!), tokenDeCancelamento);
            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListarVitrinePorVendedor(Guid? vendedorId, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ListarComCategoriaVendedorPorVendedorSemContexto(new Guid(vendedorId.ToString()!), tokenDeCancelamento);
        }

        public async Task<Produto> ListarVitrinePorId(Guid? produtoId, CancellationToken tokenDeCancelamento)
        {
            var produto = await _produtoRepository.ObterProdutoComCategoriaVendedorPorId(new Guid(produtoId.ToString()!), tokenDeCancelamento);
            return produto;
        }

        public async Task<Produto> ObterPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ObterPorId(id, tokenDeCancelamento);
        }
        public async Task AlterarStatus(Produto produto, CancellationToken tokenDeCancelamento)
        {
            var produtoOrigem = await _produtoRepository.ObterPorId(produto.Id, tokenDeCancelamento);
            if (produtoOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Produto não encontrado."));
                return;
            }

            produtoOrigem.AlterarStatus();

            await _produtoRepository.Editar(produtoOrigem, tokenDeCancelamento);
            await _produtoRepository.SalvarMudancas(tokenDeCancelamento);
        }

        public async Task<PagedResult<Produto>> ListarVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento)
        {
            var produtos = categoriaId == null ?
               await _produtoRepository.ListarComCategoriaVendedorPaginadoSemContexto(pagina, tamanho, tokenDeCancelamento) :
               await _produtoRepository.ListarComCategoriaVendedorPorCategoriaPaginadoSemContexto(new Guid(categoriaId.ToString()!), pagina, tamanho, tokenDeCancelamento);
            return produtos;
        }

        public async Task<PagedResult<Produto>> ListarVitrinePorVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento)
        {
            return await _produtoRepository.ListarComCategoriaVendedorPorVendedorPaginadoSemContexto(new Guid(vendedorId.ToString()!), pagina, tamanho, tokenDeCancelamento);
        }
    }
}
