using AutoMapper;
using LojaVirtual.Core.Business.Entities;
using LojaVirtual.Core.Business.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.List(cancellationToken));
            
            return View(categorias);
        }

        [Route("novo")]
        public IActionResult Create()
        {
            return View();
        }
        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Create(CategoriaViewModel categoriaViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(categoriaViewModel);

            await _categoriaService.Insert(_mapper.Map<Categoria>(categoriaViewModel), cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }

        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.GetById(id, cancellationToken));
            
            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.GetById(id, cancellationToken));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }
        [Route("editar/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CategoriaViewModel categoriaViewModel, CancellationToken cancellationToken)
        {
            if (id != categoriaViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(categoriaViewModel);

            var categoria = _mapper.Map<Categoria>(categoriaViewModel);
            await _categoriaService.Edit(categoria, cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.GetById(id, cancellationToken));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.GetById(id, cancellationToken));

            if (categoriaViewModel == null) return NotFound();

            await _categoriaService.Remove(id, cancellationToken);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }
    }
}
