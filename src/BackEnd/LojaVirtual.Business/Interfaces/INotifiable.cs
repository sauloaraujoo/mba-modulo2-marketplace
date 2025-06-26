using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Interfaces
{
    public interface INotifiable
    {
        bool Valid();
        bool Invalid();
        List<Notification> GetNotifications();
        void AddNotification(Notification notification);
    }
}
