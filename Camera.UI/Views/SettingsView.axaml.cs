using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;
using ReactiveUI;

namespace Camera.UI.Views
{
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.WhenActivated(i =>
            {
                this.ViewModel.LoadUser();
            });
        }
    }
}
