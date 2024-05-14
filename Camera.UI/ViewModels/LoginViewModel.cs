
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Data.Core.Plugins;
using Camera.UI.Localize;
using Camera.UI.Services;
using Joint.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Camera.UI.ViewModels
{
    public class LoginViewModel : RoutableViewModelBase
    {
        //ЗАВИСИМОТИ КОТОРЫЕ Я ПОЛУЧАЮ В КОНСТРУКТОРЕ(Microsoft.DependencyIjection САМ ЛОЖИТ СЕРВИСЫ В КОНСТРУКТОР!!!)

        #region -- Dependency --
        
        private IAuthorizationService _service;

        private INotificationService _notificationService;

        private readonly IServiceCollection _serviceProvider;

        private HomeViewModel _homeViewModel;

        private readonly INavigationService _navigationService;

        private readonly ISharedPreferences _sharedPreferences;
        #endregion

        public LoginViewModel(IScreen screen,
            INavigationService navigationService,
            IAuthorizationService service,
            INotificationService notificationService,
            IServiceCollection serviceProvider,
            ISharedPreferences sharedPreferences,
            HomeViewModel homeViewModel
        ) :
            base(screen, navigationService.RoutingState)
        {
            _homeViewModel = homeViewModel;
            _serviceProvider = serviceProvider;
            _service = service;
            _notificationService = notificationService;
            _navigationService = navigationService;
            _sharedPreferences = sharedPreferences;
            RxApp.MainThreadScheduler.Schedule(GoToProgramIfAuthorized);
            this.ConfigureValidation();
        }

        private User _user { get; set; } = new User();
        
        public string Email
        {
            get => _user.Email;
            set
            {
                _user.Email = value;
                this.RaisePropertyChanged();
            }
        }
        
        public string Password
        {
            get => _user.Password;
            set
            {
                _user.Password = value;
                this.RaisePropertyChanged();
            }
        }


        [Reactive] public bool RememberMe { get; set; } = false;
        
        //Events
        public async void Login()
        {
            if (!HasErrors)
            {
                var res = await _service.AuthorizationAsync(_user);

                if (res.IsSuccess)
                {
                    if (RememberMe)
                    {
                        await this._sharedPreferences.SaveAsync("token", _service.Token);
                    }
                    GoToHome();
                }
                else
                {
                    _notificationService.ShowError(res.Error);
                }
            }
            else
            {
                _notificationService.ShowError("Форма не валідна!");
            }
            
        }
        public void GoToRegistration() => this._navigationService.Navigate<RegistrationViewModel>();

        private void ConfigureValidation()
        {
            this.ValidationRule(x => x.Email, v =>  !string.IsNullOrEmpty(v), Resources.textEmailIsRequired);
            this.ValidationRule(x => x.Email, v => v != null &&  Regex.Match(v, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success, Resources.textEmailNotTemplate);
            this.ValidationRule(x => x.Password, v =>  !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
        }
        
        private async void GoToProgramIfAuthorized()
        {
            try
            {
                var token = await _sharedPreferences.GetAsync("token");
                if (!string.IsNullOrEmpty(token))
                {
                    this._service.SetToken(token);
                    GoToHome();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }


        private void GoToHome()
        {
            _serviceProvider.TryAddSingleton<IScreen>(_homeViewModel);
            _serviceProvider.TryAddSingleton<RoutingState>(_homeViewModel.Router);
            this.RoutingState.Navigate.Execute(_homeViewModel);
        }
        
    }
}