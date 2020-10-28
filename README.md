# Notifier
Notification pusher for .NET Core apps using SignalR.

# How to install
You can install this package via NuGet:https://www.nuget.org/packages/Notifier/

# Prerequisites
In order to use this package, you need to use SignalR in your project. Get started at: https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio

You will also need to use *Microsoft.AspNetCore.Identity*.

# Basic setup
For a default setup, you can add the following code in the *ConfigureServices* method, on *Startup.cs*:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services
        .AddNotifier<NotificationHub, User>() //this adds a default implementation of INotifier
        .UseDefaultNotificationStack(); //this adds a default implementation of INotificationStack
    ...
}
```

In the snippet above, *TNotificationHub* must inherit from *Notifier.BaseClasses.AbstractNotificationHub*, and *TUser* must be the class used to represent users in *Microsoft.AspNetCore.Identity*.

*AddNotifier* will add a default implementation of INotifier, which will be used to send notifications that implement *INotification*.

*UseDefaultNotificationStack* will add a default implementation of INotificationStack, which is responsible for stacking new notifications during *TNotificationHub* client disconnections during page reloads.

**Important**: don't forget to register *TNotificationHub* with SignalR.

```csharp
app.UseSignalR(
    routes =>
    {
        //Your other hubs...
        routes.MapHub<NotificationHub>("/notificationHub");
    }
);
```

# Front-end setup

Given you already had a connection setup for *notificationHub*, which we will name *notificationConnection* here, you will need to handle *"NotificationHandler"* events to make your notifications:
```javascript
notificationsConnection.on("NotificationHandler", function (message, notification) {
    //Do your notification here
}
```
In the snippet above, *message* is a string containing the message for your notification, and notification is the entire notification object, which you can use to pass additional information that could help you determine, for instance, if it a success, danger, warning or other kind of notification, and display them with different styles.

# Sending a notification

To start sending notifications, you should define what your notification objects would look like:

```csharp
//This is just an example
public class Notification : INotification
{
    public string Message { get; set; }

    public Notification(string message)
    {
        Message = message;
    }
    
    public string GetMessage()
    {
        return Message;
    }
}
```

Now you are ready to send notifications:

```csharp
public class SampleController : Controller
{
    //Get INotifier from DI container!
    readonly INotifier _notifier;

    public SampleController(INotifier notifier)
    {
        _notifier = notifier;
    }

    public IActionResult SampleAction()
    {
        //Send a notification
        _notifier.NotifyCurrentUser(new Notification("Sample notification"));
    }
}
```

This should work if you either make an AJAX call to SampleAction or you call it in any way that causes a page reload (form submit, hyperlinks, etc).
