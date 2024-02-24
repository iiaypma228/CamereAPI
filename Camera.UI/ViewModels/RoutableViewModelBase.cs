using System;
using ReactiveUI;

namespace Camera.UI.ViewModels;

public class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    protected RoutingState RoutingState;
    
    public RoutableViewModelBase(IScreen screen, RoutingState routingState)
    {
        HostScreen = screen;
        RoutingState = routingState;
    }
    
    
}