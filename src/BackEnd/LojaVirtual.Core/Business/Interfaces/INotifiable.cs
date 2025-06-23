using LojaVirtual.Core.Business.Notifications;

namespace LojaVirtual.Core.Business.Interfaces
{
    public interface INotifiable
    {
        bool Valid();
        bool Invalid();
        List<Notification> GetNotifications();
        void AddNotification(Notification notification);
    }
}
