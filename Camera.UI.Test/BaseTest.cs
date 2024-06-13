using Avalonia.ReactiveUI;
using Camera.UI.Views;
using OpenCvSharp;

namespace Camera.UI.Test;

public class BaseTest
{
    public MainWindow Window => AvaloniaApp.GetMainWindow();
    protected T GetWindowContentAs<T>() where T: class
    {
        return (Window.Content as RoutedViewHost).Content as T;
    }
}