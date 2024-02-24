using System;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Camera.UI.Services;

public interface INotificationService
{
    void Show(Notification notification);
    void ShowError(string text);
    void ShowInfo(string text);
}

public class NotificationService : INotificationService
{
    private readonly IServiceProvider _serviceCollection;

    private WindowNotificationManager WindowNotificationManager => _serviceCollection.GetService<WindowNotificationManager>()!;
    
    public NotificationService(IServiceProvider collection)
    {
        _serviceCollection = collection;
    }
    
    public void Show(Notification notification)
    {
        WindowNotificationManager.Show(notification);
    }

    public void ShowError(string text)
    {
        WindowNotificationManager.Show(new Notification("Error", text, NotificationType.Error));
    }

    public void ShowInfo(string text)
    {
        WindowNotificationManager.Show(new Notification("Info", text, NotificationType.Information));
    }
    
}