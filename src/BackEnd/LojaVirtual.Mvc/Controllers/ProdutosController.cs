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
            INotificavel notificavel) : base(notificavel)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Produtos", "VISUALIZAR")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken tokenDeCancelamento)
        {
            IEnumerable<ProdutoViewModel> produtos;

            if (CustomAuthorization.ValidarClaimsUsuario(this.HttpContext, "Produtos", "TODOS_PRODUTOS"))
                produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoService.ObterTodosProdutosComCategoria(tokenDeCancelamento));
            else
                produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoService.ObterTodosProdutosPropriosComCategoria(tokenDeCancelamento));



            return View(produtos);
        }

        [ClaimsAuthorize("Produtos", "ADICIONAR")]
        [Route("novo")]
        public async Task<IActionResult> Adicionar(CancellationToken tokenDeCancelamento)
        {
            var produtoViewModel = await PopularCategorias(new ProdutoViewModel(), tokenDeCancelamento);

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "ADICIONAR")]
        [HttpPost("novo")]
        public async Task<IActionResult> Adicionar(ProdutoViewModel produtoViewModel, CancellationToken tokenDeCancelamento)
        {
            produtoViewModel = await PopularCategorias(produtoViewModel, tokenDeCancelamento);
            
            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await CarregarArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }
            try
            {
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                await _produtoService.Inserir(_mapper.Map<Produto>(produtoViewModel), tokenDeCancelamento);
                if (!OperacaoValida()) return View(produtoViewModel);
            }
            catch
            {
                ExcluirArquivo(produtoViewModel.Imagem);

            }

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "VISUALIZAR")]
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Detalhes(Guid id, CancellationToken tokenDeCancelamento)
        {


            ProdutoViewModel produtoViewModel;

            if (CustomAuthorization.ValidarClaimsUsuario(this.HttpContext, "Produtos", "TODOS_PRODUTOS"))
                produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.ObterComCategoriaPorId(id, tokenDeCancelamento));
            else
                produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.ObterProprioComCategoriaPorId(id, tokenDeCancelamento));


            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produtos", "EXCLUIR")]
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Excluir(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.ObterProprioComCategoriaPorId(id, tokenDeCancelamento));

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }
        [ClaimsAuthorize("Produtos", "EXCLUIR")]
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmarExclusao(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.ObterProprioComCategoriaPorId(id, tokenDeCancelamento));

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id, tokenDeCancelamento);

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "ATUALIZAR_STATUS")]
        [HttpPost("alterar-status/{id}"), ActionName("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus(Guid id, ProdutoViewModel produtoViewModel, CancellationToken tokenDeCancelamento)
        {
            if (id != produtoViewModel.Id) return NotFound();


            var produto = _mapper.Map<Produto>(produtoViewModel);
            if (produto == null) return NotFound();

            await _produtoService.AlterarStatus(produto, tokenDeCancelamento);

            if (!OperacaoValida()) return View();

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produtos", "EDITAR")]
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Editar(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoService.ObterProprioComCategoriaPorId(id, tokenDeCancelamento));
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            produtoViewModel = await PopularCategorias(produtoViewModel, tokenDeCancelamento);

            return View(produtoViewModel);
        }
        [ClaimsAuthorize("Produtos", "EDITAR")]
        [Route("editar/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, ProdutoViewModel produtoViewModel, CancellationToken tokenDeCancelamento)
        {
            if (id != produtoViewModel.Id) return NotFound();

            produtoViewModel = await PopularCategorias(produtoViewModel, tokenDeCancelamento);

            if (!ModelState.IsValid) return View(produtoViewModel);

            var produtoOrigem = await _produtoService.ObterPorId(id, tokenDeCancelamento);
            produtoViewModel.Imagem = produtoOrigem.Imagem;
            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await CarregarArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }
            
            try
            {
                await _produtoService.Editar(_mapper.Map<Produto>(produtoViewModel), tokenDeCancelamento);
                if (!OperacaoValida() && produtoViewModel.ImagemUpload != null)
                {
                    ExcluirArquivo(produtoViewModel.Imagem);
                    return View(produtoViewModel);
                }
            }
            catch
            {
                ExcluirArquivo(produtoViewModel.Imagem);
                return View(produtoViewModel);
            }

            return RedirectToAction("Index");            
        }
        private async Task<ProdutoViewModel> PopularCategorias(ProdutoViewModel produto, CancellationToken tokenDeCancelamento)
        {
            produto.Categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.Listar(tokenDeCancelamento));
            return produto;
        }

        private async Task<bool> CarregarArquivo(IFormFile arquivo, string imgPrefixo)
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
        private void ExcluirArquivo(string imageName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            System.IO.File.Delete(path);
        }
    }
}
