using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Camera.UI.ViewModels;
using Camera.UI.Views;
using ReactiveUI;

namespace Camera.UI;

public class ViewLocator : IDataTemplate, ReactiveUI.IViewLocator
{
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
    
    public IViewFor ResolveView<T>(T viewModel, string contract = null)
    {
        var name = viewModel.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (IViewFor)Activator.CreateInstance(type)!;
        }

        throw new ArgumentException("Not finded view!");
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}