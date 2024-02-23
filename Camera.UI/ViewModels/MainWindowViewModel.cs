using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using System.Net;
using System.Reactive;

namespace Camera.UI.ViewModels;


public class MainWindowViewModel : ViewModelBase, IScreen
{

    public RoutingState Router { get; } = ViewLocator.Router;


    public MainWindowViewModel()
    {
        ViewLocator.Screen = this;
        Router.Navigate.Execute(new LoginViewModel(ViewLocator.Screen));

    }

}