using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels;

namespace Camera.UI.Views
{
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
