using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        private readonly INotificavel _notificavel;

        protected MainController(INotificavel notificavel)
        {
            _notificavel = notificavel;
        }
        protected ActionResult RespostaCustomizada(HttpStatusCode statusCode = HttpStatusCode.OK, object result = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(new
                {
                    Sucesso = true,
                    Data = result
                })
                {
                    StatusCode = (int)statusCode
                };
            }

            return BadRequest(new
            {
                Sucesso = false,
                Mensagens = _notificavel.ObterNotificacoes().Select(n => n.Message).ToArray()
            });
        }
        protected ActionResult RespostaCustomizada(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AdicionarErroProcessamento(errorMsg);
            }
            return RespostaCustomizada();
        }

        protected ActionResult RespostaCustomizada(List<Notificacao> notificacoes)
        {
            foreach (var erro in notificacoes)
            {
                AdicionarErroProcessamento(erro.Message);
            }
            return RespostaCustomizada();
        }

        protected bool OperacaoValida()
        {
            return _notificavel.Valido();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            _notificavel.AdicionarNotificacao(new Notificacao(erro));
        }        
    }
}
