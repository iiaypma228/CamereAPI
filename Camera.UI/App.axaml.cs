using System;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Camera.UI.Services;
using Camera.UI.ViewModels;
using Camera.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Camera.UI;

public partial class App : Application
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private ServiceProvider _serviceProvider = default!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(_services)
            {
            };
            
            //ОЛЕГ ЄТО КОСТІЛЬ ДЛЯ ТОГО ЧТО БІ РАБОТАЛА КРАСИВЕЙШАЯ НОТИФИКАЦИЯ НА КОТОРУЮ Я ПОТРИТИЛ ЦЕЛІЙ ДЕНЬ
            //ЭТО РЕГИСТРАЦИЯ ЗАВИСИМОСТЕЙ, ЧТО БЫ КОД БЫЛ СТРУКТУРИРОВАН
            _services.AddSingleton(i => ((MainWindow)desktop.MainWindow)._notificationManager);
            //ЗАЙДИ В ЭТОТ МЕТОД

            ConfigureServices();

            desktop.MainWindow.DataContext = this._serviceProvider.GetService<IScreen>();

            this._serviceProvider.GetService<RoutingState>()
                .Navigate.Execute(this._serviceProvider.GetService<LoginViewModel>()); // NOT NULL!
        }
        base.OnFrameworkInitializationCompleted();
    }


    private void ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        // ДОБАВЛЯЕМ ХТПП КЛИЕНТ ДЛЯ РАБОТЫ С СЕТЬЮ(ОТСЛЫАТЬ ЗАПРОСЫ НА АПИ)
        //ЕСЛИ МЫ БУДЕМ КАЖДЫЙ РАЗ СОЗДАВАТЬ ЕГО ПО НОВОМУ, БУДУТ УТЕЧКИ ПАМЯТИ
        // А ТАК ВСЕ СЕРВИСЫ БУДУТ ИСПОЛЬЗОВАТЬ ОБЩИЙ ХТТП КЛИЕНТ


        _services.AddHttpClient("", i => { i.BaseAddress = new Uri(configuration["ApiUrl"]); });
        _services.AddHttpClient<ICameraService>(i => i.BaseAddress = new Uri(configuration["ApiUrl"]))
            .AddHttpMessageHandler<BearerTokenHandler>();
            ;//.AddHttpMessageHandler<BearerTokenHandler>();
        /*    .AddHttpMessageHandler(provider =>
        {
            var accessTokenProvider = provider.GetRequiredService<IAccessTokenProvider>();
            return new BearerTokenHandler(accessTokenProvider);
        });*/


        //РЕГИСТРАЦИЯ СЕРВИСОВ, РАЗНИЦУ МЕЖДУ ТРАНЗИТ И СИНГЛТОН ПОГУГЛИ!!!
        //notify
        //_services.AddSingleton<WindowNotificationManager>();
        _services.AddTransient<INotificationService, NotificationService>();
        
        //screen
        _services.AddSingleton<RoutingState>();
        _services.AddSingleton<IScreen,MainWindowViewModel>();
        _services.AddSingleton<IServiceCollection>(_services);
        
        //ViewModels
        _services.AddSingleton<RegistrationViewModel>();
        _services.AddSingleton<LoginViewModel>();
        _services.AddSingleton<HomeViewModel>();
        _services.AddSingleton<CameraViewModel>();
        _services.AddSingleton<SettingsViewModel>();
        
        //Services        
        _services.AddSingleton<IAuthorizationService, AuthorizationService>();
        _services.AddSingleton<IRegistrationService, RegistrationService>();
        _services.AddSingleton<ICameraService, CameraService>();
        _services.AddTransient<BearerTokenHandler>();
        //_services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();

        //КОНФИГУРИРУЕМ
        _serviceProvider = _services.BuildServiceProvider();
    }
}