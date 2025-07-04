using LojaVirtual.Api.Extensions;
using LojaVirtual.Api.Models;
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
            INotifiable notifiable
        ) : base(notifiable)
        {
            _clienteService = clienteService;
        }

        [ClaimsAuthorize("Clientes", "VISUALIZAR_FAVORITOS")]
        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos(CancellationToken cancellationToken)
        {
            var favoritos = await _clienteService.GetFavoritos(cancellationToken);
            return CustomResponse(HttpStatusCode.OK, favoritos.Select(FavoritoViewModel.FromFavorito));
        }

        [ClaimsAuthorize("Clientes", "EDITAR_FAVORITOS")]
        [HttpPost("favoritos/{produtoId:guid}")]
        public async Task<IActionResult> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var adicionado = await _clienteService.AdicionarFavorito(produtoId, cancellationToken);

            if (!adicionado)
            {
                AdicionarErroProcessamento("Este produto já está nos seus favoritos.");
                return CustomResponse(HttpStatusCode.Conflict);
            }

            return CustomResponse(HttpStatusCode.Created);
        }

        [ClaimsAuthorize("Clientes", "EDITAR_FAVORITOS")]
        [HttpDelete("favoritos/{produtoId:guid}")]
        public async Task<IActionResult> RemoverFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var removido = await _clienteService.RemoverFavorito(produtoId, cancellationToken);

            if (!removido)
            {
                AdicionarErroProcessamento("Produto não estava nos favoritos.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK);
        }

    }
}
