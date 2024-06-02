using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Camera.UI.Services;
using DirectShowLib;
using Joint.Data.Constants;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels.FormsViewModels;

public class NotificationFormViewModel : RoutableViewModelBase
{
    private Joint.Data.Models.Notification _notify;

    public Joint.Data.Models.Notification Notification
    {
        get => _notify;
        set
        {
            _notify = value;
            this.RaisePropertyChanged();
        }
    }

    public string Name
    {
        get => _notify.Name;
        set
        {
            _notify.Name = value;
            this.RaisePropertyChanged();
        }
    }

    public NotificationType NotificationType
    {
        get => _notify.NotificationType;
        set
        {
            _notify.NotificationType = value;
            this.RaisePropertyChanged();
        }
    }
    public string Data
    {
        get => _notify.Data;
        set
        {
            _notify.Data = value;
            this.RaisePropertyChanged();
        }
    }
    
    
    public Action<Joint.Data.Models.Notification> NotificationCreated;


    private readonly INotificationService _notificationService;
    private readonly IServerNotificationService _serverNotificationService;

    public NotificationFormViewModel(IScreen screen, 
        RoutingState routingState, 
        INotificationService notificationService, IServerNotificationService serverNotificationService) 
        : base(screen, routingState)
    {
        _notificationService = notificationService;
        _serverNotificationService = serverNotificationService;
    }

    public async Task CameraSave()
    {
        var res = await _serverNotificationService.Save(Notification);
        if(res.IsSuccess)
        {
            if (NotificationCreated != null)
            {
                NotificationCreated.Invoke(Notification);
                NotificationCreated = null;
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