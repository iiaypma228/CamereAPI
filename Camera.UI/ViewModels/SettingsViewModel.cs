using Camera.UI.Localize;
using Camera.UI.Services;
using Joint.Data.Models;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels
{
    public class SettingsViewModel : RoutableViewModelBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationService _registrationService;
        private readonly INotificationService _notificationService;
        public SettingsViewModel(IScreen screen, RoutingState routingState, IConfiguration configuration,
            IRegistrationService registrationService,
            INotificationService notificationService) : base(screen, routingState)
        {
            _configuration = configuration;
            _registrationService = registrationService;
            _notificationService = notificationService;
            RxApp.MainThreadScheduler.Schedule(LoadUser);
            LocalizeDeffault = new KeyValuePair<string, string>(_configuration["Localize"], Localize[_configuration["Localize"]]);
            this.ConfigureValidation();
        }
        private User _user { get; set; } = new User();

        public User User 
        {
            get => _user;
            set
            {
                _user = value;
                this.RaisePropertyChanging();
            }
        }

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
        public string Phone
        {
            get => _user.Phone;
            set
            {
                _user.Phone = value;
                this.RaisePropertyChanged();
            }
        }

        [Reactive] public string RetryPassword { get; set; }

        private async void LoadUser()
        {
            _user = (await _registrationService.GetMe()).Data;
        }

        public void OpenTG()
        {
            var url = "https://t.me/OlegCameraNotification_bot";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        [Reactive] public Dictionary<string, string> Localize { get; set; } = new Dictionary<string, string> { { "uk", "Українська" }, { "en", "English (Англійська)" } };

        public KeyValuePair<string, string> LocalizeDeffault
        {
            get => _localizeDeffault;
            set
            {
                _localizeDeffault = value;
                CurrentLocale(value.Key);
                this.RaisePropertyChanged();

            }
        }
        private KeyValuePair<string, string> _localizeDeffault;

        private string CurrentLocale(string newLocale)
        {
            string res = "";
            /*     switch (newLocale)
                 {
                     case "uk":
                         {
                             res = "Українська";
                             break;
                         }
                     case "en":
                         {
                             res = "English (Англійська)";
                             break;
                         }
                     default:
                         break;
                 }*/
            _configuration["Localize"] = newLocale;
            return res;
        }

        async public void ChangeEmail()
        {
            if (HasErrors)
            {
                _notificationService.ShowError("Форма не валідна!");
                return;
            }

            _user.Email = Email;
            _user.Password = Password;

            var res = await _registrationService.ChangeUser(_user);
            if (!res.IsSuccess)
            {
                _notificationService.ShowError(res.Error);
            }
            else
            {
                _notificationService.ShowInfo("");
            }
        }

        async public void ChangePhone()
        {

            _user.Phone = Phone;
            await _registrationService.Registration(_user);
        }

        private void ConfigureValidation()
        {

            this.ValidationRule(x => x.Email, v => v != null && Regex.Match(v, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success, Resources.textEmailNotTemplate);
            this.ValidationRule(x => x.Password, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
            this.ValidationRule(x => x.RetryPassword, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
            this.ValidationRule(x => x.RetryPassword, v => v == Password, Resources.textPasswordNotEqualsRetryPassowrd);
        }
    }
}
