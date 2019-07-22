using System.Collections.Generic;

namespace Notifier.Interfaces
{
    public interface INotifier
    {
        void NotifyCurrentUser(INotification notification);
        void NotifyUser(string userId, INotification notification);
        void NotifyGroup(string groupName, INotification notification);
        void NotifyGroupExcept(string groupName, IReadOnlyList<string> connectionIds, INotification notification);
    }
}