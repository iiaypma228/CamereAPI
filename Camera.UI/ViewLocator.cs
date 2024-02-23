using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Camera.UI.ViewModels;
using ReactiveUI;

namespace Camera.UI;

public class ViewLocator : IDataTemplate
{
    public static RoutingState Router { get; } = new RoutingState();

    public static IScreen Screen { get; set; }

    public Control Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}