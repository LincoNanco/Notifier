namespace Notifier.Interfaces
{
    public interface INotifier
    {
        void NotifyCurrentUser(INotification notification);
        void NotifyUser(string userId, INotification notification);
        void NotifyGroup(string groupName, INotification notification);
    }
}