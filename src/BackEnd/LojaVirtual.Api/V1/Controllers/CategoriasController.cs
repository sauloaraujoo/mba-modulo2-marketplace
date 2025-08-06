﻿using AutoMapper;
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
        public async Task<ActionResult> Insert([FromBody] CategoriaModel request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            
            await _categoriaService.Insert(_mapper.Map<Categoria>(request), cancellationToken);                        
            
            return CustomResponse(HttpStatusCode.Created);            
        }

        [HttpGet]
        public async Task<ActionResult> List(CancellationToken cancellationToken)
        {            
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<CategoriaModel>>(await _categoriaService.List(cancellationToken)));
        }

        [ClaimsAuthorize("Categorias", "EDITAR")]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CategoriaModel request, CancellationToken cancellationToken)
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

            await _categoriaService.Edit(_mapper.Map<Categoria>(request), cancellationToken);
            
            return CustomResponse(HttpStatusCode.NoContent);            
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var categoria = _mapper.Map<CategoriaModel>(await _categoriaService.GetById(id, cancellationToken));
            
            return CustomResponse(HttpStatusCode.OK, categoria);
        }

        [ClaimsAuthorize("Categorias", "EXCLUIR")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id, CancellationToken cancellationToken)
        {
            await _categoriaService.Remove(id, cancellationToken);
            
            return CustomResponse(HttpStatusCode.NoContent);
        }
    }
}
