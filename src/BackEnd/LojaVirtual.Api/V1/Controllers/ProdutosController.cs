using AutoMapper;
using LojaVirtual.Api.Controllers;
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
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoService produtoService,
                                    INotificavel notificavel,
                                    IMapper mapper) : base(notificavel)
        {
            _produtoService = produtoService;
            _mapper = mapper;
        }
        
        [HttpGet("lista")]        
        public async Task<IActionResult> Listar(CancellationToken tokenDeCancelamento)
        {
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<ProdutoModel>>(await _produtoService.Listar(tokenDeCancelamento)));
        }
        
        [HttpPost("novo")]
        public async Task<IActionResult> Inserir([FromForm] ProdutoModel produtoModel, CancellationToken tokenDeCancelamento)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var imagePrefix = Guid.NewGuid() + "_";
            if (!await CarregarArquivo(produtoModel.ImagemUpload, imagePrefix))
            {
                return CustomResponse();
            }
            try
            {
                produtoModel.Imagem = imagePrefix + produtoModel.ImagemUpload.FileName;
                await _produtoService.Inserir(_mapper.Map<Produto>(produtoModel), tokenDeCancelamento);
                if(!OperacaoValida())
                {
                    ApagarArquivo(produtoModel.Imagem);
                }                
            }
            catch 
            {
                ApagarArquivo(produtoModel.Imagem);

            }            
            return CustomResponse(HttpStatusCode.Created);
        }

        [HttpPut("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, [FromForm] ProdutoModel produtoModel, CancellationToken tokenDeCancelamento)
        {
            if (id != produtoModel.Id)
            {
                AdicionarErroProcessamento("O id informado não é o mesmo que foi passado no form");
                return CustomResponse();
            }

            var produtoOrigem = await _produtoService.ObterProdutoProprioPorId(id, tokenDeCancelamento);
            if (produtoOrigem is null)
            {
                return CustomResponse();
            }

            produtoModel.Imagem = produtoOrigem.Imagem;
            ModelState.Remove("ImagemUpload");

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (produtoModel.ImagemUpload != null)
            {
                var imagePrefix = Guid.NewGuid() + "_";
                if (!await CarregarArquivo(produtoModel.ImagemUpload, imagePrefix))
                {
                    return CustomResponse();
                }

                produtoModel.Imagem = imagePrefix + produtoModel.ImagemUpload.FileName;
            }

            try
            {
                await _produtoService.Editar(_mapper.Map<Produto>(produtoModel), tokenDeCancelamento);
                if (!OperacaoValida() && produtoModel.ImagemUpload != null)
                {
                    ApagarArquivo(produtoModel.Imagem);
                }
            }
            catch
            {
                if (produtoModel.ImagemUpload != null)
                {
                    ApagarArquivo(produtoModel.Imagem);
                }

            }
            return CustomResponse(HttpStatusCode.NoContent);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = _mapper.Map<ProdutoModel>(await _produtoService.ObterProdutoProprioPorId(id, tokenDeCancelamento));
            return CustomResponse(HttpStatusCode.OK, produto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id, CancellationToken tokenDeCancelamento)
        {
            await _produtoService.Remover(id, tokenDeCancelamento);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        private void ApagarArquivo(string imageName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            System.IO.File.Delete(path);            
        }
        private async Task<bool> CarregarArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                AdicionarErroProcessamento("Forneça uma imagem para este produto!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                AdicionarErroProcessamento("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
