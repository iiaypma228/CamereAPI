using System;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Camera.UI.Services;


public interface INavigationService
{
    RoutingState RoutingState { get; }
    void Navigate<TViewModel>() where TViewModel : IRoutableViewModel ;
}

public class NavigationService : INavigationService
{
    public RoutingState RoutingState { get; }
    private readonly IServiceProvider _serviceProvider;
    
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RoutingState = _serviceProvider.GetService<RoutingState>();
    }

    public void Navigate<TViewModel>() where TViewModel : IRoutableViewModel
    {
        this.RoutingState.Navigate.Execute(_serviceProvider.GetService<TViewModel>());
    }
}