using System;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Formatters;

namespace Camera.UI.Views
{
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public LoginView()
        {
            InitializeComponent();
        }
    }
}
