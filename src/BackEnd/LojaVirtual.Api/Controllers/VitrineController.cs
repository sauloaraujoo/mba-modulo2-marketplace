using AutoMapper;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Common;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/vitrine")]
    public class VitrineController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;
        public VitrineController(IProdutoService produtoService,
                                 ICategoriaService categoriaService,
                                 INotifiable notifiable,
                                 IMapper mapper) : base(notifiable)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [HttpGet("")]        
        public async Task<ActionResult> ListVitrine(Guid? categoriaId, CancellationToken cancellationToken, [FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {            
            var resultado = await _produtoService.ListVitrinePaginado(categoriaId, pagina, tamanho, cancellationToken);
            var viewModel = new PagedResult<ProdutoModel>
            {
                TotalItens = resultado.TotalItens,
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                Itens = _mapper.Map<IEnumerable<ProdutoModel>>(resultado.Itens)
            };
            return CustomResponse(HttpStatusCode.OK, viewModel);
            //return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<ProdutoModel>>(await _produtoService.ListVitrine(categoriaId, cancellationToken)));
        }

        [HttpGet("por-vendedor/{vendedorId:guid}")]
        public async Task<ActionResult> ListVitrineByVendedor([FromRoute]Guid vendedorId, CancellationToken cancellationToken, [FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            var resultado = await _produtoService.ListVitrineByVendedorPaginado(vendedorId, pagina, tamanho, cancellationToken);

            var viewModel = new PagedResult<ProdutoModel>
            {
                TotalItens = resultado.TotalItens,
                PaginaAtual = resultado.PaginaAtual,
                TamanhoPagina = resultado.TamanhoPagina,
                Itens = _mapper.Map<IEnumerable<ProdutoModel>>(resultado.Itens)
            };
            return CustomResponse(HttpStatusCode.OK, viewModel);

            //return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<ProdutoModel>>(await _produtoService.ListVitrineByVendedor(vendedorId, cancellationToken)));
        }

        [HttpGet("detalhe/{id:Guid}")]                
        public async Task<IActionResult> GetDetailById(Guid id, CancellationToken cancellationToken)
        {
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<ProdutoModel>(await _produtoService.ListVitrineById(id,cancellationToken)));            
        }

        [HttpGet("categorias")]
        public async Task<ActionResult> ListarCategorias(CancellationToken cancellationToken)
        {
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<CategoriaModel>>(await _categoriaService.List(cancellationToken)));
        }

    }
}
