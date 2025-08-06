using LojaVirtual.Api.Models;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/vendedor")]
    public class VendedorController(IVendedorService vendedorService,
                                    INotificavel notifiable) : MainController(notifiable)
    {
        private readonly IVendedorService _vendedorService = vendedorService;

        [HttpGet("{id:Guid}")]        
        public async Task<ActionResult> ObterInformacoes(Guid id, 
                                                         CancellationToken cancellationToken)
        {            
            return CustomResponse(HttpStatusCode.OK, 
                                  VendedorViewModel.FromVendedor(await _vendedorService.ObterPorId(id, 
                                                                                                cancellationToken)));
        }
    }
}
