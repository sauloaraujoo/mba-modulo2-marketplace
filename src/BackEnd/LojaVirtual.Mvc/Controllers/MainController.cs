using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Controllers
{
    public abstract class MainController : Controller
    {
        private readonly INotificavel _notificavel;

        protected MainController(INotificavel notificavel)
        {
            _notificavel = notificavel;
        }
        protected bool OperacaoValida()
        {
            return _notificavel.Valido();
        }
    }
}
