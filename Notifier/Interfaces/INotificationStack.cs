using System.Collections.Generic;

namespace Notifier.Interfaces
{
    public interface INotificationStack
    {
        void AddNotification(INotification notification);
        List<INotification> GetNotifications();
        void ClearNotifications();
    }
}