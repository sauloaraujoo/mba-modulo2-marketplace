using AutoMapper;
using LojaVirtual.Api.Models;
using LojaVirtual.Core.Business.Interfaces;
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
        private readonly IMapper _mapper;
        public VitrineController(IProdutoService produtoService,
                                 INotifiable notifiable,
                                 IMapper mapper) : base(notifiable)
        {
            _produtoService = produtoService;
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
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<ProdutoModel>(await _produtoService.GetById(id,cancellationToken)));            
        }        
    }
}
