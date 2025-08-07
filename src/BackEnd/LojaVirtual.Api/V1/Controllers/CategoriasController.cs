using AutoMapper;
using LojaVirtual.Api.Controllers;
using LojaVirtual.Api.Extensions;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categorias")]
    public class CategoriasController : MainController
    {
        private readonly ICategoriaService _categoriaService;        
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaService categoriaService,
                                    INotificavel notifiable,
                                    IMapper mapper): base(notifiable)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Categorias", "ADICIONAR")]
        [HttpPost]
        public async Task<ActionResult> Inserir([FromBody] CategoriaModel request, CancellationToken tokenDeCancelamento)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            
            await _categoriaService.Inserir(_mapper.Map<Categoria>(request), tokenDeCancelamento);                        
            
            return CustomResponse(HttpStatusCode.Created);            
        }

        [HttpGet]
        public async Task<ActionResult> Listar(CancellationToken tokenDeCancelamento)
        {            
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<CategoriaModel>>(await _categoriaService.Listar(tokenDeCancelamento)));
        }

        [ClaimsAuthorize("Categorias", "EDITAR")]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] CategoriaModel request, CancellationToken tokenDeCancelamento)
        {
            if (id != request.Id)
            {
                AdicionarErroProcessamento("O id informado não é o mesmo que foi passado no body");                
                return CustomResponse();
            }
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _categoriaService.Editar(_mapper.Map<Categoria>(request), tokenDeCancelamento);
            
            return CustomResponse(HttpStatusCode.NoContent);            
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> ObterPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoria = _mapper.Map<CategoriaModel>(await _categoriaService.ObterPorId(id, tokenDeCancelamento));
            
            return CustomResponse(HttpStatusCode.OK, categoria);
        }

        [ClaimsAuthorize("Categorias", "EXCLUIR")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id, CancellationToken tokenDeCancelamento)
        {
            await _categoriaService.Remover(id, tokenDeCancelamento);
            
            return CustomResponse(HttpStatusCode.NoContent);
        }
    }
}
