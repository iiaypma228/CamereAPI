using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels.FormsViewModels;

namespace Camera.UI.Views.FormsViews;

public partial class NotificationFormView : ReactiveUserControl<NotificationFormViewModel>
{
    public NotificationFormView()
    {
        InitializeComponent();
    }
}