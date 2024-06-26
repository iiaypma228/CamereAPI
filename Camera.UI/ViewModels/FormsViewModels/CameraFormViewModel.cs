﻿using Camera.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using DirectShowLib;
using Joint.Data.Constants;
using Joint.Data.Models;

namespace Camera.UI.ViewModels.FormsViewModels
{
    public class CameraFormViewModel : RoutableViewModelBase
    {
        private Joint.Data.Models.Camera _camera;
        private readonly NotificationViewModel _notificationViewModel;

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

        public CameraConnection CameraConnection
        {
            get => _camera.Connection;
            set
            {
                _camera.Connection = value;
                this.RaisePropertyChanged();
            }
        }

        public string CameraName
        {
            get => _camera.Name;
            set
            {
                _camera.Name = value;
                this.RaisePropertyChanged();
            }
        }

        [Reactive]
        public ObservableCollection<Joint.Data.Models.Notification> CameraNotification { get; set; } =
            new ObservableCollection<Notification>();
        [Reactive] public Notification SelectedNotification { get; set; }
        [Reactive] public ObservableCollection<DsDevice> LocalCameras { get; set; }
        
        
        public Action<Joint.Data.Models.Camera> CameraCreated;


        private readonly ICameraService _cameraService;
        private readonly INotificationService _notificationService;
        private readonly IServerNotificationService _serverNotificationService;

        public CameraFormViewModel(IScreen screen, 
            RoutingState routingState, 
            ICameraService service, 
            INotificationService notificationService,
            IServerNotificationService serverNotificationService, NotificationViewModel notificationViewModel) 
            : base(screen, routingState)
        {
            _cameraService = service;
            _notificationService = notificationService;
            _serverNotificationService = serverNotificationService;
            _notificationViewModel = notificationViewModel;
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

        public void AddNotification()
        {
            _notificationViewModel.SelectableMode = true;
            this.RoutingState.Navigate.Execute(_notificationViewModel);
            _notificationViewModel.NotificationSelected = (async notification  => 
            {
                if (this.CameraNotification.Any(i => i.Id == notification.Id))
                {
                    _notificationService.ShowError("Оповіщення вже прив\'язано до цієї камери!");
                    return;
                }

                Camera.Notifications.Add(notification);
                CameraNotification.Add(notification);
                this.RoutingState.NavigateBack.Execute();
                Dispatcher.UIThread.Post(() =>
                {
                    _notificationViewModel.SelectableMode = false;
                });
                
            });
        }

        public async void UnlinkNotification()
        {
            if (this.SelectedNotification == null)
            {
                this._notificationService.ShowError("Оберіть камеру!");
            }
            else
            {
                Camera.Notifications.Remove(SelectedNotification);
                CameraNotification.Remove(SelectedNotification);
            }
        }
        
        public void Cancel()
        {
            this.RoutingState.NavigateBack.Execute();
        }


        private async void LoadCameraNotification()
        { 
            LocalCameras =
                new ObservableCollection<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            this.CameraNotification.Clear();
            var notifications = (await this._serverNotificationService.GetByCamera(this.Camera.Id)).Data;
            
           this.CameraNotification = new ObservableCollection<Notification>(notifications);
           Camera.Notifications ??= new List<Notification>();
           Camera.Notifications.Clear();
           Camera.Notifications.AddRange(notifications);
        }
    }
}
