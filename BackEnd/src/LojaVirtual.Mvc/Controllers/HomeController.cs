using AutoMapper;
using LojaVirtual.Core.Business.Interfaces;
using LojaVirtual.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    public class HomeController : MainController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;

        public HomeController(ILogger<HomeController> logger,
                              IMapper mapper, 
                              INotifiable notifiable,
                              IProdutoService produtoService,
                              ICategoriaService categoriaService) : base(notifiable)
        {
            _logger = logger;
            _mapper = mapper;
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}        

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index(Guid? categoriaId, CancellationToken cancellationToken)
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoVitrineViewModel>>(await _produtoService.ListVitrine(categoriaId, cancellationToken));
            var _categorias = _mapper.Map<IEnumerable<CategoriaViewModel>>(await _categoriaService.List(cancellationToken));
            ViewBag.Categorias = _categorias; // Passar categorias para a view
            
            return View(produtos);
        }
        public async Task<IActionResult> ListaProdutos(Guid? categoriaId, CancellationToken cancellationToken)
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoVitrineViewModel>>(await _produtoService.ListVitrine(categoriaId, cancellationToken));

            return PartialView("_ListaProdutos", produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id, CancellationToken cancellationToken)
        {            
            var produto = await _produtoService.GetById(id, cancellationToken);

            if (produto == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProdutoViewModel>(produto));
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
