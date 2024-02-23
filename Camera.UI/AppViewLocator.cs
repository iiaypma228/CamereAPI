using Camera.UI.ViewModels;
using Camera.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI
{
    public class AppViewLocator : ReactiveUI.IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            LoginViewModel context => new LoginView { DataContext = context },
            RegistrationViewModel context => new Registration() { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
