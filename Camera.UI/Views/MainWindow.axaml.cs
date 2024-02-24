using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Camera.UI.Views;

public partial class MainWindow : Window
{
    public WindowNotificationManager _notificationManager;
    private IServiceCollection _collection;
    public MainWindow(IServiceCollection collection)
    {
        _collection = collection;
        AvaloniaXamlLoader.Load(this);
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        _notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 5
        };
        _collection.TryAddSingleton(_notificationManager);
        
    }
    
}