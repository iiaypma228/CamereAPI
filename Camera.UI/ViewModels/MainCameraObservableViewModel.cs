using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Camera.UI.Extensions;
using Camera.UI.Services;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Joint.Data.Constants;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Camera.UI.ViewModels;

public partial class MainCameraObservableViewModel : RoutableViewModelBase
{
    private readonly ICameraObservableService _service;
    private readonly ICameraService _cameraService;
    private readonly INotificationService _notificationService;

    private Joint.Data.Models.Camera? _selectedCamera = null;
    [Reactive] public Bitmap VideoFrame { get; set; }
    [Reactive] public bool IsOpened { get; set; }
    [Reactive] public ObservableCollection<Joint.Data.Models.Camera> Cameras { get; set; }

    public Joint.Data.Models.Camera? SelectedCamera
    {
        get => _selectedCamera;
        set
        {
            if (value != null && value != _selectedCamera)
            {
                _selectedCamera = value;
                this.RaisePropertyChanged();
                if (this.ConnectCameraCommand.CanBeCanceled)
                {
                    this.ConnectCameraCommand.Cancel();
                }
                this.ConnectCameraCommand.Execute(null);
            }

        }
    }

    public bool ReactOnMotion
    {
        get => _service.ReactToMotion;
        set
        {
            this._service.ReactToMotion = value;
            this.RaisePropertyChanged();
        }
    }
    
    public MainCameraObservableViewModel(
        ICameraObservableService cameraObservableService,
        ICameraService cameraService,
        INotificationService notificationService,
        IScreen screen, 
        RoutingState routingState
        ) : base(screen, routingState)
    {
        _service = cameraObservableService;
        _cameraService = cameraService;
        _notificationService = notificationService;
        
        //
        /*this.WhenValueChanged(x => x.SelectedCamera).Subscribe(v =>
        {
            if (SelectedCamera != v)
                this.ConnectCamera();
        });*/
        
        _cameraService.CamerasObservable.Subscribe(i =>
        {
            Cameras = i;
            if (this.SelectedCamera is null)
            {
                this.SelectedCamera = Cameras.FirstOrDefault();
                if (this.ConnectCameraCommand.CanBeCanceled)
                {
                    this.ConnectCameraCommand.Cancel();
                }
                this.ConnectCameraCommand.Execute(null);
            }
            
        });
        cameraObservableService.ImageGrab += ((sender, args) => VideoFrame = (args as GrabImageEventArgs)!.Bitmap);
        
    }

    [RelayCommand]
    private async Task ConnectCamera()
    {
         if (SelectedCamera is null)
            return;

        await Task.Run(() =>
        {
            IsOpened = false;
            if (SelectedCamera.Connection == CameraConnection.Cabel)
            {
                if (int.TryParse(this.SelectedCamera.ConnectionData, out int result))
                {
                    IsOpened = _service.StartObservable(SelectedCamera);
                }
                else
                {
                    _notificationService.ShowError("Данні для підключення не корректні!");
                    IsOpened = false;
                }
            }
            else
            {
                IsOpened = _service.StartObservable(SelectedCamera);
            }
        });
        
    }
    


    
    
}