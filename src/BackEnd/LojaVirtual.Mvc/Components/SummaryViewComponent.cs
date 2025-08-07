using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Components
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificavel _notificavel;

        public SummaryViewComponent(INotificavel notificavel)
        {
            _notificavel = notificavel;
        }

        public IViewComponentResult Invoke()
        {
            var notificacoes = _notificavel.ObterNotificacoes();
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }
    }
}
