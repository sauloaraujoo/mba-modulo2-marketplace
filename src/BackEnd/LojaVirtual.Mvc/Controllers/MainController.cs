using LojaVirtual.Core.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    public abstract class MainController : Controller
    {
        private readonly INotifiable _notifiable;

        protected MainController(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }
        protected bool OperacaoValida()
        {
            return _notifiable.Valid();
        }
    }
}
