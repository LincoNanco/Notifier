using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifier.BaseClasses;
using Notifier.Interfaces;

namespace Notifier
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services to the DI container.
        /// </summary>
        public static IServiceCollection AddNotifier<THub, TUser>(this IServiceCollection services) where THub : AbstractNotificationHub<TUser> where TUser : class 
        {
            //Add an implementation of IViewRenderService only if it is not already provided
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<INotifier, Notifier<THub, TUser>>();
            return services;
        }

        public static IServiceCollection UseDefaultNotificationStack(this IServiceCollection services)
        {
            services.AddSingleton<INotificationStack, NotificationStack>();
            return services;
        }


        public static IServiceCollection UseNotificationStack<TStack>(this IServiceCollection services) where TStack : class, INotificationStack
        {
            services.AddSingleton<INotificationStack, TStack>();
            return services;
        }
    }
}