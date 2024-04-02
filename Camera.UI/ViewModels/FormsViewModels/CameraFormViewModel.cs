using Camera.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Joint.Data.Models;

namespace Camera.UI.ViewModels.FormsViewModels
{
    public class CameraFormViewModel : RoutableViewModelBase
    {
        private Joint.Data.Models.Camera _camera;

        public Joint.Data.Models.Camera Camera
        {
            get => _camera;
            set
            {
                _camera = value;
                this.LoadCameraNotification();
                this.RaisePropertyChanged();
            }
        }

        [Reactive] public ObservableCollection<Joint.Data.Models.Notification> CameraNotification { get; set; }
        
        public Action<Joint.Data.Models.Camera> CameraCreated;


        private readonly ICameraService _cameraService;
        private readonly INotificationService _notificationService;
        private readonly IServerNotificationService _serverNotificationService;

        public CameraFormViewModel(IScreen screen, 
            RoutingState routingState, 
            ICameraService service, 
            INotificationService notificationService,
            IServerNotificationService serverNotificationService
            ) 
            : base(screen, routingState)
        {
            _cameraService = service;
            _notificationService = notificationService;
            _serverNotificationService = serverNotificationService;
            //RxApp.MainThreadScheduler.Schedule(LoadCameraNotification);
        }

        public async Task CameraSave()
        {
            var res = await _cameraService.SaveCamera(Camera);
            if(res.IsSuccess)
            {
                if (CameraCreated != null)
                {
                    CameraCreated.Invoke(Camera);
                    CameraCreated = null;
                }
                this.RoutingState.NavigateBack.Execute();

            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
        }

        public void Cancel()
        {
            this.RoutingState.NavigateBack.Execute();
        }


        private async void LoadCameraNotification()
        {
            this.CameraNotification = new ObservableCollection<Notification>((await this._serverNotificationService.GetNotification(this.Camera.Id)).Data!);
        }
    }
}
