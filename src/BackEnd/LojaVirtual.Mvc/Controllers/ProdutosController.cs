using AutoMapper;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Services;
using LojaVirtual.Mvc.Extensions;
using LojaVirtual.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    [Route("meus-produtos")]
    [Authorize]
    public class ProdutosController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoService produtoService,
            ICategoriaService categoriaService,
            IMapper mapper,
            INotifiable notifiable) : base(notifiable)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Produtos", "VI")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            IEnumerable<ProdutoViewModel> produtos;

            if (CustomAuthorization.ValidarClaimsUsuario(this.HttpContext, "Produtos", "TODOS_PRODUTOS"))
                produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoService.GetAllProdutoWithCategoria(cancellationToken));
            else
                produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoService.GetAllSelfProdutoWithCategoria(cancellationToken));



            return View(produtos);
        }

        [ClaimsAuthorize("Produos", "AD")]
        [Route("novo")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var produtoViewModel = await PopularCategorias(new ProdutoViewModel(), cancellationToken);

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "AD")]
        [HttpPost("novo")]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel, CancellationToken cancellationToken)
        {
            produtoViewModel = await PopularCategorias(produtoViewModel, cancellationToken);
            
            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadFile(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }
            try
            {
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                await _produtoService.Insert(_mapper.Map<Produto>(produtoViewModel), cancellationToken);
                if (!OperacaoValida()) return View(produtoViewModel);
            }
            catch
            {
                DeleteFile(produtoViewModel.Imagem);

            }

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "VI")]
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {


            ProdutoViewModel produtoViewModel;

            if (CustomAuthorization.ValidarClaimsUsuario(this.HttpContext, "Produtos", "TODOS_PRODUTOS"))
                produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.GetWithCategoriaById(id, cancellationToken));
            else
                produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.GetSelfWithCategoriaById(id, cancellationToken));


            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "EX")]
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.GetSelfWithCategoriaById(id, cancellationToken));

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }
        [ClaimsAuthorize("Produtos", "EX")]
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.GetSelfWithCategoriaById(id, cancellationToken));

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remove(id, cancellationToken);

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "ATUALIZAR_STATUS")]
        [HttpPost]
        [ActionName("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus(Guid id, ProdutoViewModel produtoViewModel, CancellationToken cancellationToken)
        {
            if (id != produtoViewModel.Id) return NotFound();


            var produto = _mapper.Map<Produto>(produtoViewModel);
            if (produto == null) return NotFound();

            await _produtoService.AlterarStatus(produto, cancellationToken);

            if (!OperacaoValida()) return View();

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "ED")]
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.GetSelfWithCategoriaById(id, cancellationToken));
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            produtoViewModel = await PopularCategorias(produtoViewModel, cancellationToken);

            return View(produtoViewModel);
        }
        [ClaimsAuthorize("Produtos", "ED")]
        [Route("editar/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel, CancellationToken cancellationToken)
        {
            if (id != produtoViewModel.Id) return NotFound();

            produtoViewModel = await PopularCategorias(produtoViewModel, cancellationToken);

            if (!ModelState.IsValid) return View(produtoViewModel);

            var produtoOrigem = await _produtoService.GetById(id, cancellationToken);
            produtoViewModel.Imagem = produtoOrigem.Imagem;
            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadFile(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }
            
            try
            {
                await _produtoService.Edit(_mapper.Map<Produto>(produtoViewModel), cancellationToken);
                if (!OperacaoValida() && produtoViewModel.ImagemUpload != null)
                {
                    DeleteFile(produtoViewModel.Imagem);
                    return View(produtoViewModel);
                }
            }
            catch
            {
                DeleteFile(produtoViewModel.Imagem);
                return View(produtoViewModel);
            }

            return RedirectToAction("Index");            
        }
        private async Task<ProdutoViewModel> PopularCategorias(ProdutoViewModel produto, CancellationToken cancellationToken)
        {
            produto.Categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.List(cancellationToken));
            return produto;
        }

        private async Task<bool> UploadFile(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Forneça uma imagem para este produto!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
        private void DeleteFile(string imageName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            System.IO.File.Delete(path);
        }
    }
}
