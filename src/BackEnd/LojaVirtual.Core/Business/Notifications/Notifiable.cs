using LojaVirtual.Core.Business.Interfaces;

namespace LojaVirtual.Core.Business.Notifications
{
    public class Notifiable : INotifiable
    {
        private readonly List<Notification> _notifications;
        public Notifiable()
        {
            _notifications = new List<Notification>();
        }
        public IReadOnlyCollection<Notification> Notifications { get { return _notifications; } }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }
        public List<Notification> GetNotifications()
        {
            return _notifications;
        }
        public bool Invalid()
        {
            return _notifications.Any();
        }
        public bool Valid()
        {
            return !Invalid();
        }
    }
}
