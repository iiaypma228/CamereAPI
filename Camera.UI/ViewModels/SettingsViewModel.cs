using Camera.UI.Services;
using Joint.Data.Models;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels
{
    public  class SettingsViewModel : RoutableViewModelBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationService _registrationService;
        public SettingsViewModel(IScreen screen, RoutingState routingState, IConfiguration configuration,
            IRegistrationService registrationService) : base(screen, routingState)
        {
            _configuration = configuration;
            _registrationService = registrationService;
            LocalizeDeffault = new KeyValuePair<string, string>(_configuration["Localize"], Localize[_configuration["Localize"]]);
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

        public void OpenTG()
        {
            var url = "https://t.me/OlegCameraNotification_bot";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        [Reactive] public Dictionary<string, string> Localize { get; set; } = new Dictionary<string, string> { { "uk", "Українська" }, { "en","English (Англійська)" } };

        public KeyValuePair<string, string> LocalizeDeffault { 
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
            var user = await _registrationService.GetMe();
            if (user.IsSuccess)
            {
                user.Data.Email = Email;
                await _registrationService.Registration(user.Data);
            }
        }

        async public void ChangePhone()
        {
            var user = await _registrationService.GetMe();
            if (user.IsSuccess)
            {
                user.Data.Phone = Phone;
                await _registrationService.Registration(user.Data);
            }
        }
    }
}
