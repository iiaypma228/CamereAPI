using System;
using Camera.UI.Services;
using Camera.UI.ViewModels.FormsViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels;

public class CameraViewModel : RoutableViewModelBase
{
    [Reactive] public ObservableCollection<Joint.Data.Models.Camera> Items { get; set; } = new();
    [Reactive] public Joint.Data.Models.Camera SelectedCamera { get; set; }
    private ICameraService _service;
    private INotificationService _notificationService;
    private CameraFormViewModel _cameraFormViewModel;

    public CameraViewModel(IScreen screen, RoutingState routingState, 
        ICameraService service, 
        INotificationService notificationService,
        CameraFormViewModel cameraFormViewModel) : base(screen, routingState)
    {
        _service = service;
        _notificationService = notificationService;
        _cameraFormViewModel = cameraFormViewModel;
        //RxApp.MainThreadScheduler.Schedule(LoadCameras);
        _service.CamerasObservable.Subscribe( 
            onNext: i => this.Items = i,
            onError: exception => _notificationService.ShowError(exception.Message) 
            );
        
        //For data grid row double click
        EditCameraCommand = ReactiveCommand.Create<object,Unit>(EditCamera);
    }

    public ReactiveCommand<object, Unit> EditCameraCommand { get;  }
    
    public void CreateCamera()
    {
        _cameraFormViewModel.Camera = new Joint.Data.Models.Camera();
        RoutingState.Navigate.Execute(_cameraFormViewModel);

    }
    public async void DeleteCamera()
    {
        if (_cameraFormViewModel.Camera == null)
        {
            _notificationService.ShowError("Камера не обрана!");
        }
        else 
        {
            var res = await _service.DeleteCamera(SelectedCamera.Id.ToString());
            if (res.IsSuccess)
            {
                Items.Remove(SelectedCamera);
                SelectedCamera = null;
            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
        }
    }
    
    private Unit EditCamera(object p)
    {
        if (SelectedCamera == null)
        {
            _notificationService.ShowError("Камера не обрана!");
        }
        else
        {
            _cameraFormViewModel.Camera = SelectedCamera;
            RoutingState.Navigate.Execute(_cameraFormViewModel);
        }

        return Unit.Default;
    }
    
}