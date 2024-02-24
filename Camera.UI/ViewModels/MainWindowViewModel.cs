using System;
using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Camera.UI.ViewModels;


public class MainWindowViewModel : ViewModelBase, IScreen
{

    public RoutingState Router { get; }


    public MainWindowViewModel(RoutingState routingState, INotificationService notificationService)
    {
        RxApp.DefaultExceptionHandler = Observer.Create<Exception>(i => notificationService.ShowError(i.Message));
        Router = routingState;
    }

}