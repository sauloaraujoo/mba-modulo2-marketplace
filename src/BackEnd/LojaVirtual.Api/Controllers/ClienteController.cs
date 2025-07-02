using LojaVirtual.Api.Extensions;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace LojaVirtual.Api.Controllers
{
    [Authorize]
    [Route("api/cliente")]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IProdutoService _produtoService;

        public ClienteController(IClienteService clienteService,
                                 IProdutoService produtoService,
                                 INotifiable notifiable) : base(notifiable)
        {
            _clienteService = clienteService;
            _produtoService = produtoService;
        }

        [ClaimsAuthorize("Clientes", "VISUALIZAR_FAVORITOS")]
        [HttpGet("favoritos")]
        public async Task<IActionResult> GetFavoritos(CancellationToken cancellationToken)
        {
            var clienteId = GetClienteId();
            var favoritos = await _clienteService.GetFavoritos(clienteId, cancellationToken);

            var lista = new List<FavoritoViewModel>();
            foreach (var fav in favoritos)
            {
                var produto = await _produtoService.GetById(fav.ProdutoId, cancellationToken);
                lista.Add(new FavoritoViewModel
                {
                    ClienteId = fav.ClienteId,
                    ProdutoId = fav.ProdutoId,
                    ProdutoNome = produto.Nome,
                    ProdutoImagem = produto.Imagem,
                    ProdutoPreco = produto.Preco
                });
            }

            return CustomResponse(HttpStatusCode.OK, lista);
        }

        [ClaimsAuthorize("Clientes", "EDITAR_FAVORITOS")]
        [HttpPost("favoritos/{produtoId:guid}")]
        public async Task<IActionResult> AdicionarFavorito(Guid produtoId, CancellationToken cancellationToken)
        {
            var clienteId = GetClienteId();
            var adicionado = await _clienteService.AdicionarFavorito(clienteId, produtoId, cancellationToken);

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
            var clienteId = GetClienteId();
            await _clienteService.RemoverFavorito(clienteId, produtoId, cancellationToken);
            return CustomResponse(HttpStatusCode.NoContent);
        }

        private Guid GetClienteId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new InvalidOperationException("Usuário logado não possui claim sub.");

            return Guid.Parse(idClaim);
        }

    }
}
