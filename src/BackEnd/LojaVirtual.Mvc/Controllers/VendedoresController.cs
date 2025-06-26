using AutoMapper;
using LojaVirtual.Core.Business.Entities;
using LojaVirtual.Core.Business.Interfaces;
using LojaVirtual.Core.Business.Services;
using LojaVirtual.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Mvc.Controllers
{
    [Route("vendedores")]
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var venderores = _mapper.Map<IEnumerable<VendedorViewModel>>(await _vendedorService.List(cancellationToken));

            return View(venderores);
        }


        [Route("vendedor/alterar-status/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AlterarStatus(Guid id,VendedorViewModel vendedorViewModel, CancellationToken cancellationToken)
        {
            if (id != vendedorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View();

            vendedorViewModel.Ativo = !vendedorViewModel.Ativo;

            var vendedor = _mapper.Map<Vendedor>(vendedorViewModel);
            if (vendedor == null) return NotFound();

            await _vendedorService.Edit(vendedor, cancellationToken);

            if (!OperacaoValida()) return View();

            return RedirectToAction("Index");
        }

    }
}
