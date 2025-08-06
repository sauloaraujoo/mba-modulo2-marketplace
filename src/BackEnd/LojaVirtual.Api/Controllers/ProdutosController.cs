using AutoMapper;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [Authorize]
    [Route("api/produtos")]
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
        public async Task<IActionResult> List(CancellationToken tokenDeCancelamento)
        {
            return CustomResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<ProdutoModel>>(await _produtoService.List(tokenDeCancelamento)));
        }
        
        [HttpPost("novo")]
        public async Task<IActionResult> Insert([FromForm] ProdutoModel produtoModel, CancellationToken tokenDeCancelamento)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var imagePrefix = Guid.NewGuid() + "_";
            if (!await UploadFile(produtoModel.ImagemUpload, imagePrefix))
            {
                return CustomResponse();
            }
            try
            {
                produtoModel.Imagem = imagePrefix + produtoModel.ImagemUpload.FileName;
                await _produtoService.Insert(_mapper.Map<Produto>(produtoModel), tokenDeCancelamento);
                if(!OperacaoValida())
                {
                    DeleteFile(produtoModel.Imagem);
                }                
            }
            catch 
            {
                DeleteFile(produtoModel.Imagem);

            }            
            return CustomResponse(HttpStatusCode.Created);
        }

        [HttpPut("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromForm] ProdutoModel produtoModel, CancellationToken tokenDeCancelamento)
        {
            if (id != produtoModel.Id)
            {
                AdicionarErroProcessamento("O id informado não é o mesmo que foi passado no form");
                return CustomResponse();
            }

            var produtoOrigem = await _produtoService.GetSelfProdutoById(id, tokenDeCancelamento);
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
                if (!await UploadFile(produtoModel.ImagemUpload, imagePrefix))
                {
                    return CustomResponse();
                }

                produtoModel.Imagem = imagePrefix + produtoModel.ImagemUpload.FileName;
            }

            try
            {
                await _produtoService.Edit(_mapper.Map<Produto>(produtoModel), tokenDeCancelamento);
                if (!OperacaoValida() && produtoModel.ImagemUpload != null)
                {
                    DeleteFile(produtoModel.Imagem);
                }
            }
            catch
            {
                if (produtoModel.ImagemUpload != null)
                {
                    DeleteFile(produtoModel.Imagem);
                }

            }
            return CustomResponse(HttpStatusCode.NoContent);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id, CancellationToken tokenDeCancelamento)
        {
            var produto = _mapper.Map<ProdutoModel>(await _produtoService.GetSelfProdutoById(id, tokenDeCancelamento));
            return CustomResponse(HttpStatusCode.OK, produto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id, CancellationToken tokenDeCancelamento)
        {
            await _produtoService.Remove(id, tokenDeCancelamento);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        private void DeleteFile(string imageName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            System.IO.File.Delete(path);            
        }
        private async Task<bool> UploadFile(IFormFile arquivo, string imgPrefixo)
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
