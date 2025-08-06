using AutoMapper;
using LojaVirtual.Api.Extensions;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [Authorize]
    [Route("api/categorias")]
    public class CategoriasController : MainController
    {
        private readonly ICategoriaService _categoriaService;        
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaService categoriaService,
                                    INotifiable notifiable,
                                    IMapper mapper): base(notifiable)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Categorias", "ADICIONAR")]
        [HttpPost]
        public async Task<ActionResult> Inserir([FromBody] CategoriaModel request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            
            await _categoriaService.Inserir(_mapper.Map<Categoria>(request), cancellationToken);                        
            
            return CustomResponse(HttpStatusCode.Created);            
        }

        [HttpGet]
        public async Task<ActionResult> Listar(CancellationToken cancellationToken)
        {            
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<CategoriaModel>>(await _categoriaService.List(cancellationToken)));
        }

        [ClaimsAuthorize("Categorias", "EDITAR")]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] CategoriaModel request, CancellationToken cancellationToken)
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

            await _categoriaService.Editar(_mapper.Map<Categoria>(request), cancellationToken);
            
            return CustomResponse(HttpStatusCode.NoContent);            
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> ObterId(Guid id, CancellationToken cancellationToken)
        {
            var categoria = _mapper.Map<CategoriaModel>(await _categoriaService.ObterPorId(id, cancellationToken));
            
            return CustomResponse(HttpStatusCode.OK, categoria);
        }

        [ClaimsAuthorize("Categorias", "EXCLUIR")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remover(Guid id, CancellationToken cancellationToken)
        {
            await _categoriaService.Remove(id, cancellationToken);
            
            return CustomResponse(HttpStatusCode.NoContent);
        }
    }
}
