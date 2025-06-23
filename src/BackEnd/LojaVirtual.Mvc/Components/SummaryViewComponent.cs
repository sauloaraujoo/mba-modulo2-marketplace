using LojaVirtual.Core.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Mvc.Components
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifiable _notifiable;

        public SummaryViewComponent(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }

        public IViewComponentResult Invoke()
        {
            var notificacoes = _notifiable.GetNotifications();
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }
    }
}
