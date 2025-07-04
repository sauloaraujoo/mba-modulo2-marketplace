using AutoMapper;
using LojaVirtual.Api.Models;
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
        public async Task<ActionResult> ListVitrine(Guid? categoriaId, CancellationToken cancellationToken)
        {            
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<ProdutoModel>>(await _produtoService.ListVitrine(categoriaId, cancellationToken)));
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
