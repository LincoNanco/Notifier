using Notifier.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace Notifier
{
    public class Notifier<THub,TUser> : INotifier where THub : Hub where TUser : class
    {
        readonly IHubContext<THub> _hubContext;
        readonly HttpContext _context;
        readonly INotificationStack _stack;

        /// <summary>
        /// Sens notifications to users using a SignalR Hub that inherits from AbstractNotificationHub.
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="hubContext"></param>
        /// <param name="stack"></param>
        /// <param name="userManager"></param>
        public Notifier(IHttpContextAccessor accessor, IHubContext<THub> hubContext, INotificationStack stack)
        {
            _hubContext = hubContext;
            _context = accessor.HttpContext;
            _stack = stack;
        }

        /// <summary>
        /// Notifies the ClaimsPrincipal user
        /// </summary>
        /// <param name="notification"></param>
        public void NotifyCurrentUser(INotification notification)
        {
            ClaimsPrincipal principal = _context.User;
            //If the request was made through AJAX, send the response inmediatly.
            if (_context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                _hubContext.Clients
                .User(principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value)
                .SendAsync("NotificationHandler", notification.GetMessage(), notification);
            }
            else
            {
                //If not, add to stack and send it after next reload.
                _stack.AddNotification(notification);
            }
        }

        /// <summary>
        /// Adds a new notification to the stack
        /// </summary>
        /// <param name="notification"></param>
        public void AddToNotificationStack(INotification notification)
        {
            _stack.AddNotification(notification);
        }

        /// <summary>
        /// Notifies a group based on its name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="notification"></param>
        public void NotifyGroup(string groupName, INotification notification)
        {
            _hubContext.Clients.Group(groupName).SendAsync("NotificationHandler", notification.GetMessage(), notification);
        }

        /// <summary>
        /// Notifies a group based on its name, except for a list of users identified by their connection ids.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="ConnectionIds"></param>
        /// <param name="notification"></param>
        public void NotifyGroupExcept(string groupName, IReadOnlyList<string> ConnectionIds, INotification notification)
        {
            _hubContext.Clients.GroupExcept(groupName, ConnectionIds).SendAsync("NotificationHandler", notification.GetMessage(), notification);
        }

        /// <summary>
        /// Notifies a specific user based on its user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notification"></param>
        public void NotifyUser(string userId, INotification notification)
        {
            _hubContext.Clients.User(userId).SendAsync("NotificationHandler", notification.GetMessage(), notification);
        }
    }
}

