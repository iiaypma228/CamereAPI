using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Camera.UI.Services;
using Camera.UI.ViewModels.FormsViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels;

public class NotificationViewModel : RoutableViewModelBase
{
    private readonly IServerNotificationService _service;
    private readonly INotificationService _notificationService;
    private NotificationFormViewModel _notificationFormViewModel;
    [Reactive] public ObservableCollection<Joint.Data.Models.Notification> Items { get; set; } = new();
    [Reactive] public Joint.Data.Models.Notification SelectedNotification { get; set; }

    [Reactive] public bool SelectableMode { get; set; } = false;
    
    public Action<Joint.Data.Models.Notification> NotificationSelected;
    public ReactiveCommand<object, Unit> EditNotificationCommand { get;  }
    
    public NotificationViewModel(IScreen screen, RoutingState routingState, IServerNotificationService service, INotificationService notificationService, NotificationFormViewModel notificationFormViewModel) : base(screen, routingState)
    {
        _service = service;
        _notificationService = notificationService;
        _notificationFormViewModel = notificationFormViewModel;
        EditNotificationCommand = ReactiveCommand.Create<object,Unit>(EditCamera);
        _service.NotificationsObservable.Subscribe( 
            onNext: i => this.Items = i,
            onError: exception => _notificationService.ShowError(exception.Message) 
        );
    }

    public void Back()
    {
        SelectableMode = false;
        this.RoutingState.NavigateBack.Execute();
    }

    public void SelectedConfirm()
    {
        if (SelectedNotification == null)
        {
            _notificationService.ShowError("Оповіщення не обрано!");
        }
        else
        {
            NotificationSelected(SelectedNotification);
        }
    }
    
    public void CreateNotification()
    {
        _notificationFormViewModel.Notification = new Joint.Data.Models.Notification();
        RoutingState.Navigate.Execute(_notificationFormViewModel);
    }
    public async void DeleteCamera()
    {
        if (SelectedNotification == null)
        {
            _notificationService.ShowError("Оповіщення не обрано!");
        }
        else 
        {
            var res = await _service.Delete(SelectedNotification);
            if (res.IsSuccess)
            {
                Items.Remove(SelectedNotification);
                SelectedNotification = null;
            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
        }
    }
    
    private Unit EditCamera(object p)
    {
        if (SelectedNotification == null)
        {
            _notificationService.ShowError("Камера не обрана!");
        }
        else
        {
            _notificationFormViewModel.Notification = SelectedNotification;
            RoutingState.Navigate.Execute(_notificationFormViewModel);
        }

        return Unit.Default;
    }
    
}