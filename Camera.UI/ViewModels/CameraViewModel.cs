using ReactiveUI;

namespace Camera.UI.ViewModels;

public class CameraViewModel : RoutableViewModelBase
{
    public CameraViewModel(IScreen screen, RoutingState routingState) : base(screen, routingState)
    {
    }
}