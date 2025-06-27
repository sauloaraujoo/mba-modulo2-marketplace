using AutoMapper;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace LojaVirtual.Api.Controllers
{
    [Authorize]
    [Route("api/favoritos")]
    public class FavoritosController : MainController
    {
        private readonly IFavoritoService _favoritoService;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public FavoritosController(
            IFavoritoService favoritoService,
            IProdutoService produtoService,
            INotifiable notifiable,
            IMapper mapper) : base(notifiable)
        {
            _favoritoService = favoritoService;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] FavoritoViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var clienteId = ObterClienteId();

            var produto = await _produtoService.GetById(viewModel.ProdutoId, cancellationToken);

            if (produto == null || !produto.Ativo)
            {
                AdicionarErroProcessamento("Produto não encontrado ou inativo.");
                return CustomResponse();
            }

            await _favoritoService.AdicionarFavorito(clienteId, viewModel.ProdutoId, cancellationToken);

            return CustomResponse(HttpStatusCode.Created);
        }

        [HttpDelete("{produtoId:Guid}")]
        public async Task<IActionResult> Remover(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = ObterClienteId();

            await _favoritoService.RemoverFavorito(clienteId, produtoId, cancellationToken);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancellationToken)
        {
            var clienteId = ObterClienteId();

            var produtos = await _favoritoService.ListarFavoritos(clienteId, cancellationToken);

            var result = produtos.Select(p => new FavoritoProdutoViewModel
            {
                ProdutoId = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Imagem = p.Imagem,
                Preco = p.Preco
            });

            return CustomResponse(HttpStatusCode.OK, result);
        }

        private Guid ObterClienteId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id);
        }
    }
}
