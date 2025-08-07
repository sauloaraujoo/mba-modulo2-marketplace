using LojaVirtual.Api.Controllers;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.V1.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vendedores")]
    public class VendedorController(IVendedorService vendedorService,
                                    INotificavel notificavel) : MainController(notificavel)
    {
        private readonly IVendedorService _vendedorService = vendedorService;

        [HttpGet("{id:Guid}")]        
        public async Task<ActionResult> ObterInformacoes(Guid id, 
                                                         CancellationToken tokenDeCancelamento)
        {            
            return CustomResponse(HttpStatusCode.OK, 
                                  VendedorViewModel.FromVendedor(await _vendedorService.ObterPorId(id,
                                                                                                tokenDeCancelamento)));
        }
    }
}
