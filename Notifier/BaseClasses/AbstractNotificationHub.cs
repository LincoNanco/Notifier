using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Notifier.Interfaces;

namespace Notifier.BaseClasses
{
    public abstract class AbstractNotificationHub<TUser> : Hub where TUser : class
    {
        INotifier _notifier;
        INotificationStack _stack;
        public AbstractNotificationHub(INotifier notifier, INotificationStack stack)
        {
            _notifier = notifier;
            _stack = stack;
        }

        /// <summary>
        /// Resend notifications that weren't able to send before due to connection delay
        /// </summary>
        /// <returns></returns>
        protected async Task SendPendingNotifications()
        {
            List<INotification> notifications = _stack.GetNotifications();
            foreach(INotification n in notifications)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("NotificationHandler", n.GetMessage(), n);
            }
            _stack.ClearNotifications();
        }

        /// <summary>
        /// Adding current user to their corresponding groups
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await SendPendingNotifications();
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Removing current user from their corresponding groups
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}