using System;
using ReactiveUI;

namespace Camera.UI.ViewModels;

public class RoutableViewModelBase : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    protected RoutingState RoutingState;
    
    public RoutableViewModelBase(IScreen screen, RoutingState routingState)
    {
        HostScreen = screen;
        RoutingState = routingState;
    }

    public ViewModelActivator Activator { get; } = new ViewModelActivator();
}