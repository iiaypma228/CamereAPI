using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;

namespace Camera.UI.Views
{
    internal partial class RegistrationView : ReactiveUserControl<RegistrationViewModel>
    {
        public RegistrationView()
        {
            InitializeComponent();
        }
    }
}
