using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels;

public class HomeViewModel : RoutableViewModelBase, IScreen
{
    private IServiceProvider _serviceProvider;
    
    public HomeViewModel(IScreen screen, RoutingState routingState, IServiceProvider serviceProvided) : base(screen, routingState)
    {
        _serviceProvider = serviceProvided;
    }
    
    [Reactive] public bool IsPaneOpen { get; set; }= false;
    
    private MenuItemTemplate? _selectedListItem;

    public MenuItemTemplate? SelectedListItem
    {
        get => _selectedListItem;
        set
        {
            this.Router.Navigate.Execute((RoutableViewModelBase)_serviceProvider.GetService(value.ModelType));
            this.RaiseAndSetIfChanged(ref _selectedListItem, value);
        }
    }
    
    public RoutingState Router { get; } = new RoutingState();
    
    public void TriggerPane() => IsPaneOpen = !IsPaneOpen;
    
    public ObservableCollection<MenuItemTemplate> Items { get; } = new()
    {
        new MenuItemTemplate(typeof(CameraViewModel), "CameraIcon"),
        new MenuItemTemplate(typeof(SettingsViewModel), "SettingsIcon")
    };
    
}
public class MenuItemTemplate
{
    private const string StreamGeometryNotFound =
        "M24 4C35.0457 4 44 12.9543 44 24C44 35.0457 35.0457 44 24 44C12.9543 44 4 35.0457 4 24C4 12.9543 12.9543 4 24 4ZM24 6.5C14.335 6.5 6.5 14.335 6.5 24C6.5 33.665 14.335 41.5 24 41.5C33.665 41.5 41.5 33.665 41.5 24C41.5 14.335 33.665 6.5 24 6.5ZM24.25 32C25.0784 32 25.75 32.6716 25.75 33.5C25.75 34.3284 25.0784 35 24.25 35C23.4216 35 22.75 34.3284 22.75 33.5C22.75 32.6716 23.4216 32 24.25 32ZM24.25 13C27.6147 13 30.5 15.8821 30.5 19.2488C30.502 21.3691 29.7314 22.7192 27.8216 24.7772L26.8066 25.8638C25.7842 27.0028 25.3794 27.7252 25.3409 28.5793L25.3379 28.7411L25.3323 28.8689L25.3143 28.9932C25.2018 29.5636 24.7009 29.9957 24.0968 30.0001C23.4065 30.0049 22.8428 29.4493 22.8379 28.7589C22.8251 26.9703 23.5147 25.7467 25.1461 23.9739L26.1734 22.8762C27.5312 21.3837 28.0012 20.503 28 19.25C28 17.2634 26.2346 15.5 24.25 15.5C22.3307 15.5 20.6142 17.1536 20.5055 19.0587L20.4935 19.3778C20.4295 20.0081 19.8972 20.5 19.25 20.5C18.5596 20.5 18 19.9404 18 19.25C18 15.8846 20.8864 13 24.25 13Z";

    public MenuItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "");

        // TODO: see if there's a better way to look for a resource by key
        Application.Current!.TryFindResource(iconKey, out var res);
        var streamGeometry = res as StreamGeometry ?? StreamGeometry.Parse(StreamGeometryNotFound);
        ListItemIcon = streamGeometry;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
}