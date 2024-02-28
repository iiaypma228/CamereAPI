using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;

namespace Camera.UI.Views;

internal partial class CameraView : ReactiveUserControl<CameraViewModel>
{
    public CameraView()
    {
        InitializeComponent();
    }
}