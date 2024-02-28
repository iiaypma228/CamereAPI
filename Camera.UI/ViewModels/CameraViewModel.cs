using Camera.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels;

public class CameraViewModel : RoutableViewModelBase
{
    [Reactive] public ObservableCollection<Joint.Data.Models.Camera> Items { get; set; } = new();
    private ICameraService _service;
    private INotificationService _notificationService;

    public CameraViewModel(IScreen screen, RoutingState routingState, ICameraService service, INotificationService notificationService) : base(screen, routingState)
    {
        _service = service;
        _notificationService = notificationService;
    }

    public async Task LoadCameras()
    {
        var cameras = await _service.GetCameras();
        if (cameras.IsSuccess)
        {
            Items = new ObservableCollection<Joint.Data.Models.Camera>(cameras.Data);
        }
        else
        {
            _notificationService.ShowError(cameras.Error);
        }
    }
}