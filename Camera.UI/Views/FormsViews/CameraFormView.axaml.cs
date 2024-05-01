using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.ViewModels.FormsViewModels;
using DirectShowLib;
using DynamicData.Binding;
using Joint.Data.Constants;
using ReactiveUI;

namespace Camera.UI.Views.FormsViews
{
    public partial class CameraFormView : ReactiveUserControl<CameraFormViewModel>
    {
        public CameraFormView()
        {
            InitializeComponent();

            this.WhenAnyValue(i => i.ViewModel.CameraConnection).Subscribe(i =>
            {
                if (i == CameraConnection.Cabel)
                {
                    EthernetTextBox.IsVisible = false;
                    LocalCamerasListBox.IsVisible = true;
                }
                else
                {
                    EthernetTextBox.IsVisible = true;
                    LocalCamerasListBox.IsVisible = false;
                }
            });
        }

        private void LocalCamerasListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var cameraComboBox = sender as ComboBox;

            this.ViewModel.CameraName = (cameraComboBox.SelectedItem as DsDevice).Name;
            this.ViewModel.Camera.ConnectionData = cameraComboBox.SelectedIndex.ToString();
        }
    }
}
