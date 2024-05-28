using System;
using System.Collections.ObjectModel;
using Camera.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels;

public class NotificationViewModel : RoutableViewModelBase
{
    private readonly IServerNotificationService _service;
    private readonly INotificationService _notificationService;
    [Reactive] public ObservableCollection<Joint.Data.Models.Notification> Items { get; set; } = new();
    [Reactive] public Joint.Data.Models.Notification SelectedNotification { get; set; }

    
    public NotificationViewModel(IScreen screen, RoutingState routingState, IServerNotificationService service, INotificationService notificationService) : base(screen, routingState)
    {
        _service = service;
        _notificationService = notificationService;
        _service.NotificationsObservable.Subscribe( 
            onNext: i => this.Items = i,
            onError: exception => _notificationService.ShowError(exception.Message) 
        );
    }
}