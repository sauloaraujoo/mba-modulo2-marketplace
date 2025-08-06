using LojaVirtual.Api.Extensions;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Common;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [Authorize]
    [Route("api/cliente")]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(
            IClienteService clienteService,
            INotificavel notificavel
        ) : base(notificavel)
        {
            _clienteService = clienteService;
        }

        [ClaimsAuthorize("Clientes", "VISUALIZAR_FAVORITOS")]
        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos(CancellationToken tokenDeCancelamento)
        {
            var favoritos = await _clienteService.GetFavoritos(tokenDeCancelamento);
            return CustomResponse(HttpStatusCode.OK, favoritos.Select(FavoritoViewModel.FromFavorito));
        }

        [ClaimsAuthorize("Clientes", "VISUALIZAR_FAVORITOS")]
        [HttpGet("favorito")]
        public async Task<IActionResult> GetFavoritosPaginado(CancellationToken tokenDeCancelamento, [FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            var resultado = await _clienteService.GetFavoritosPaginado(pagina, tamanho, tokenDeCancelamento);
            var viewModel = new PagedResult<FavoritoViewModel>
            {
                TotalItens = resultado.TotalItens,
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                Itens = resultado.Itens.Select(FavoritoViewModel.FromFavorito)
            };
            return CustomResponse(HttpStatusCode.OK, viewModel);
        }

        [ClaimsAuthorize("Clientes", "EDITAR_FAVORITOS")]
        [HttpPost("favoritos/{produtoId:guid}")]
        public async Task<IActionResult> AdicionarFavorito(Guid produtoId, CancellationToken tokenDeCancelamento)
        {
            var adicionado = await _clienteService.AdicionarFavorito(produtoId, tokenDeCancelamento);

            if (!adicionado)
            {
                AdicionarErroProcessamento("Este produto já está nos seus favoritos.");
                return CustomResponse(HttpStatusCode.Conflict);
            }

            return CustomResponse(HttpStatusCode.Created);
        }

        [ClaimsAuthorize("Clientes", "EDITAR_FAVORITOS")]
        [HttpDelete("favoritos/{produtoId:guid}")]
        public async Task<IActionResult> RemoverFavorito(Guid produtoId, CancellationToken tokenDeCancelamento)
        {
            var removido = await _clienteService.RemoverFavorito(produtoId, tokenDeCancelamento);

            if (!removido)
            {
                AdicionarErroProcessamento("Produto não estava nos favoritos.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK);
        }

    }
}
