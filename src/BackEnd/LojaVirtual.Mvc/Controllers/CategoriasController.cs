using AutoMapper;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;
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
            INotificavel notificavel) : base(notificavel)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Categorias", "VISUALIZAR")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken tokenDeCancelamento)
        {
            var categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.Listar(tokenDeCancelamento));
            
            return View(categorias);
        }

        [ClaimsAuthorize("Categorias", "ADICIONAR")]
        [Route("novo")]
        public IActionResult Adicionar()
        {
            return View();
        }

        [ClaimsAuthorize("Categorias", "ADICIONAR")]
        [Route("novo")]
        [HttpPost]
        public async Task<IActionResult> Adicionar(CategoriaViewModel categoriaViewModel, CancellationToken tokenDeCancelamento)
        {
            if (!ModelState.IsValid) return View(categoriaViewModel);

            await _categoriaService.Inserir(_mapper.Map<Categoria>(categoriaViewModel), tokenDeCancelamento);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Categorias", "ADICIONAR")]
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Detalhes(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, tokenDeCancelamento));
            
            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "EDITAR")]
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Editar(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, tokenDeCancelamento));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "EDITAR")]
        [Route("editar/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, CategoriaViewModel categoriaViewModel, CancellationToken tokenDeCancelamento)
        {
            if (id != categoriaViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(categoriaViewModel);

            var categoria = _mapper.Map<Categoria>(categoriaViewModel);
            await _categoriaService.Editar(categoria, tokenDeCancelamento);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Categorias", "EXCLUIR")]
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Excluir(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, tokenDeCancelamento));

            if (categoriaViewModel == null)
            {
                return NotFound();
            }

            return View(categoriaViewModel);
        }

        [ClaimsAuthorize("Categorias", "EXCLUIR")]
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmarExclusao(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoriaViewModel = _mapper.Map<CategoriaViewModel>(await _categoriaService.ObterPorId(id, tokenDeCancelamento));

            if (categoriaViewModel == null) return NotFound();

            await _categoriaService.Remover(id, tokenDeCancelamento);

            if (!OperacaoValida()) return View(categoriaViewModel);

            return RedirectToAction("Index");
        }
    }
}
