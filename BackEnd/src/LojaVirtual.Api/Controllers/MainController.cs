using LojaVirtual.Core.Business.Interfaces;
using LojaVirtual.Core.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace LojaVirtual.Api.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        private readonly INotifiable _notifiable;

        protected MainController(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }
        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object result = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(result)
                {
                    StatusCode = (int)statusCode
                };
            }
            
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", _notifiable.GetNotifications().Select(n => n.Message).ToArray() }
            }));
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AdicionarErroProcessamento(errorMsg);
            }
            return CustomResponse();
        }

        protected ActionResult CustomResponse(List<Notification> notifications)
        {
            foreach (var erro in notifications)
            {
                AdicionarErroProcessamento(erro.Message);
            }
            return CustomResponse();
        }

        protected bool OperacaoValida()
        {
            return _notifiable.Valid();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            _notifiable.AddNotification(new Notification(erro));
        }        
    }
}
