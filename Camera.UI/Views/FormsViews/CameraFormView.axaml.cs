using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels.FormsViewModels;

namespace Camera.UI.Views.FormsViews
{
    public partial class CameraFormView : ReactiveUserControl<CameraFormViewModel>
    {
        public CameraFormView()
        {
            InitializeComponent();
        }
    }
}
