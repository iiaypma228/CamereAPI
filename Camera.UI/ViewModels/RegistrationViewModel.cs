using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera.UI.Localize;
using ReactiveUI.Validation.Extensions;
using System.Text.RegularExpressions;

namespace Camera.UI.ViewModels
{
    public class RegistrationViewModel : RoutableViewModelBase
    {
        private User _user { get; set; } = new User();

        private readonly IRegistrationService _service;
        private readonly INotificationService _notificationService;
        //private LoginViewModel _loginViewModel;
        
        public string Email
        {
            get => _user.Email;
            set
            {
                _user.Email = value;
                _user.Login = value;
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
        
        [Reactive] public string RetryPassword { get; set; }
        

        public RegistrationViewModel(IScreen screen, 
            RoutingState routingState,
            //LoginViewModel loginViewModel,
            IRegistrationService registrationService,
            INotificationService notificationService)
            : base(screen, routingState)
        {
            _service = registrationService;
            _notificationService = notificationService;
            this.WhenAnyValue(x => x.Email, x => x.Password, x => x.RetryPassword).Skip(1).Take(1).Subscribe(onNext: i => ConfigureValidation());
        }


        //Event
        public async void Registration()
        {
            if (!HasErrors)
            {
                var res = await _service.Registration(_user);

                if (res.IsSuccess)
                {
                    _notificationService.ShowInfo("Ви зареєструвалися!");
                    GoToLogin();
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
        public void GoToLogin() => this.RoutingState.NavigateBack.Execute();

        
        private void ConfigureValidation()
        {
            if (ValidationContext.Validations.Count > 0)
            {
                this.ValidationRule(x => x.Email, v =>  !string.IsNullOrEmpty(v), Resources.textEmailIsRequired);
                this.ValidationRule(x => x.Email, v => v != null && Regex.Match(v, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success, Resources.textEmailNotTemplate);
                this.ValidationRule(x => x.Password, v =>  !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
                this.ValidationRule(x => x.RetryPassword, v =>  !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
                this.ValidationRule(x => x.RetryPassword, v =>  v == Password, Resources.textPasswordNotEqualsRetryPassowrd);

            }
        }
    }
}
