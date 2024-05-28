using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;

namespace Camera.UI.Views;

public partial class NotificationView : ReactiveUserControl<NotificationViewModel>
{
    public NotificationView()
    {
        InitializeComponent();
    }
}