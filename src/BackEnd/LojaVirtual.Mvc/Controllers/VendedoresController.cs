using AutoMapper;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Mvc.Extensions;
using LojaVirtual.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    [Authorize]
    [Route("vendedores")]
    public class VendedoresController : MainController
    {
        private readonly IVendedorService _vendedorService;
        private readonly IMapper _mapper;

        public VendedoresController(IVendedorService vendedorService,
                                    IMapper mapper,
                                    INotifiable notifiable) : base(notifiable)
        {
            _vendedorService = vendedorService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Vendedores", "VISUALIZAR")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken tokenDeCancelamento)
        {
            var vendedores = _mapper.Map<IEnumerable<VendedorViewModel>>(await _vendedorService.List(tokenDeCancelamento));

            return View(vendedores);
        }

        [ClaimsAuthorize("Vendedores", "ATUALIZAR_STATUS")]
        [HttpPost("alterar-status/{id}"), ActionName("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus(Guid id, 
                                                       VendedorViewModel vendedorViewModel, 
                                                       CancellationToken tokenDeCancelamento)
        {
            if (id != vendedorViewModel.Id) return NotFound();

            var vendedor = _mapper.Map<Vendedor>(vendedorViewModel);
            if (vendedor == null) return NotFound();

            await _vendedorService.AlterarStatus(vendedor, tokenDeCancelamento);

            if (!OperacaoValida()) return View();

            return RedirectToAction("Index");
        }
    }
}
