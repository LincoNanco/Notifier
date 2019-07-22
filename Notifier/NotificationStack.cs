using System.Collections.Generic;
using Notifier.Interfaces;

namespace Notifier
{
    public class NotificationStack : INotificationStack
    {
        List<INotification> notifications = new List<INotification>();
        
        public void AddNotification(INotification notification)
        {
            notifications.Add(notification);
        }

        public List<INotification> GetNotifications()
        {
            return notifications;
        }

        public void ClearNotifications()
        {
            notifications = new List<INotification>();
        }
    }
}