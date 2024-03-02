using Camera.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels.FormsViewModels
{
    public class CameraFormViewModel : RoutableViewModelBase
    {
        [Reactive] public Joint.Data.Models.Camera Camera { get; set; }
        public Action<Joint.Data.Models.Camera> CameraCreated;


        private readonly ICameraService _cameraService;
        private readonly INotificationService _notificationService;
        

        public CameraFormViewModel(IScreen screen, RoutingState routingState, ICameraService service, INotificationService notificationService) : base(screen, routingState)
        {
            _cameraService = service;
            _notificationService = notificationService; 
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

    }
}
