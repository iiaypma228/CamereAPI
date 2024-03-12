using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;
using LibVLCSharp.Avalonia;

namespace Camera.UI.Views;

public partial class MainCameraObservableView : ReactiveUserControl<MainCameraObservableViewModel>
{
    public MainCameraObservableView()
    {
        InitializeComponent();
    }
}