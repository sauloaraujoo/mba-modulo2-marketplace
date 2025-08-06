using AutoMapper;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Mvc.Extensions;
using LojaVirtual.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    [Route("categorias")]
    [Authorize]
    public class CategoriasController : MainController
    {   
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaService categoriaService, 
            IMapper mapper,
            INotifiable notifiable) : base(notifiable)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Categorias", "VI")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.List(cancellationToken));
            
            return View(categorias);
        }

        [ClaimsAuthorize("Categorias", "AD")]
        [Route("novo")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Categorias", "AD")]
        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Criar(CategoriaViewModel categoriaViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(categoriaViewModel);

            await _categoriaService.Inserir(_mapper.Map<Categoria>(categoriaViewModel), cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Categorias", "AD")]
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Detalhar(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, cancellationToken));
            
            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "ED")]
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Editar(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, cancellationToken));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "ED")]
        [Route("editar/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, CategoriaViewModel categoriaViewModel, CancellationToken cancellationToken)
        {
            if (id != categoriaViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(categoriaViewModel);

            var categoria = _mapper.Map<Categoria>(categoriaViewModel);
            await _categoriaService.Editar(categoria, cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Categorias", "EX")]
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, cancellationToken));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "EX")]
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ExcluirConfirmado(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, cancellationToken));

            if (categoriaViewModel == null) return NotFound();

            await _categoriaService.Remove(id, cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }
    }
}
