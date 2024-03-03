using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace Camera.UI.Views
{
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public LoginView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                //this.Bind
                
                this.BindValidation(ViewModel, x => x.User.Email, x => x.UserPasswordValidation.Text)
                    .DisposeWith(disposables);
            });
        }
    }
}
